using System.Linq;
using HarmonyLib;

namespace NoCultivatorDust;

[HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
static class ZNetSceneAwakePatch
{
    static void Postfix(ZNetScene __instance)
    {
        // For each PieceTable and for each Piece in each PieceTable remove the place effect
        var cultivator = __instance.GetPrefab("Cultivator").GetComponent<ItemDrop>();
        var cultivatorPieceTable = cultivator.m_itemData.m_shared.m_buildPieces;
        if (cultivatorPieceTable == null) return;
        foreach (var piece in cultivatorPieceTable.m_pieces)
        {
            var prefab = ZNetScene.instance.GetPrefab(Utils.GetPrefabName(piece));
            if (prefab == null) continue;
            prefab.TryGetComponent<Piece>(out var pieceComp);
            if (pieceComp == null) continue;
            pieceComp.m_placeEffect.m_effectPrefabs = pieceComp.m_placeEffect.m_effectPrefabs
                .Where(effect => !effect.m_prefab.name.Contains("vfx")).ToArray();
            NoCultivatorDustPlugin.NoCultivatorDustLogger.LogDebug("Removed build dust from " +
                                                                   piece.gameObject.name +
                                                                   " Current list of effect prefabs: " +
                                                                   string.Join("\n",
                                                                       pieceComp.m_placeEffect
                                                                           .m_effectPrefabs
                                                                           .Select(
                                                                               effect => effect.m_prefab
                                                                                   .name)));
        }
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.PlacePiece))]
static class PiecePlacePiecePatch
{
    static void Prefix(Player __instance, ref Piece piece)
    {
        // cache the piece.gameObject.name
        string pieceName = piece.gameObject.name;
        if (!pieceName.Contains("replant") && !pieceName.Contains("cultivate")) return;
        NoCultivatorDustPlugin.NoCultivatorDustLogger.LogDebug("Preventing cultivator dust from spawning " + pieceName);
        piece.m_placeEffect.m_effectPrefabs = piece.m_placeEffect.m_effectPrefabs
            .Where(effect => !effect.m_prefab.name.Contains("vfx")).ToArray();
        NoCultivatorDustPlugin.NoCultivatorDustLogger.LogDebug("Removed build dust from " + pieceName +
                                                               " Current list of effect prefabs: " + string.Join("\n",
                                                                   piece.m_placeEffect.m_effectPrefabs.Select(
                                                                       effect => effect.m_prefab.name)));
    }
}