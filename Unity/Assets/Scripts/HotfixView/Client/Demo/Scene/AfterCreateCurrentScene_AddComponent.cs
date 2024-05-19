namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterCreateCurrentScene_AddComponent: AEvent<Scene, AfterCreateCurrentSceneAddComponent>
    {
        protected override async ETTask Run(Scene scene, AfterCreateCurrentSceneAddComponent args)
        {
            scene.AddComponent<ResourcesLoaderComponent>();
            scene.AddComponent<PrefabPool, ResourcesLoaderComponent>(scene.GetComponent<ResourcesLoaderComponent>());
            await ETTask.CompletedTask;
        }
    }
}