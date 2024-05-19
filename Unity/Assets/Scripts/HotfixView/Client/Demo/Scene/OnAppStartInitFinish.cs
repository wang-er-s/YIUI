namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class OnAppStartInitFinish: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await SceneChangeHelper.SceneChangeTo(root, CurrentSceneType.Block, IdGenerater.Instance.GenerateId());
            await ETTask.CompletedTask;
        }
    }
}