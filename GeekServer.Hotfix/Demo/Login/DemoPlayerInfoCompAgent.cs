using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    public class DemoPlayerInfoCompAgent : QueryComponentAgent<DemoPlayerInfoComp>
    {
        public async Task<long> GetRoleIdOfPlayer(string userName, int sdkType)
        {
            var playerId = $"{sdkType}_{userName}";
            if (Comp.PlayerMap.TryGetValue(playerId, out var state))
            {
                if (state.RoleMap.TryGetValue(Settings.Ins.ServerId, out var roleId))
                    return roleId;
                return 0;
            }
            state = await Comp.LoadState<DemoPlayerInfoState>(playerId, () =>
            {
                return new DemoPlayerInfoState()
                {
                    Id = playerId,
                    UserName = userName,
                    SdkType = sdkType
                };
            });

            Comp.PlayerMap[playerId] = state;
            if (state.RoleMap.TryGetValue(Settings.Ins.ServerId, out var roleId2))
                return roleId2;
            return 0;
        }

        public Task CreateRoleToPlayer(string userName, int sdkType, long roleId)
        {
            var playerId = $"{sdkType}_{userName}";
            Comp.PlayerMap.TryGetValue(playerId, out var state);
            if (state == null)
            {
                state = new DemoPlayerInfoState();
                state.Id = playerId;
                state.SdkType = sdkType;
                state.UserName = userName;
                Comp.PlayerMap[playerId] = state;
            }
            state.RoleMap[Settings.Ins.ServerId] = roleId;
            state.UpdateChangeVersion();//state肯定有变化，这里不做完全处理
            return Comp.SaveState<DemoPlayerInfoState>(playerId, state);
        }
    }
}
