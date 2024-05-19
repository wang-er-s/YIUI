namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterCreateCurrentScene_Block_LogicView : AEvent<Scene, AfterCreateCurrentSceneLogic>
    {
        protected override async ETTask Run(Scene scene, AfterCreateCurrentSceneLogic args)
        {
            if(scene.Name != CurrentSceneType.Block) return;
            await ETTask.CompletedTask; 
        }
    }
}