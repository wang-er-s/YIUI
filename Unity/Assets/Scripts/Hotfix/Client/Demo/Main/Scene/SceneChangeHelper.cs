namespace ET.Client
{
    public static partial class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, string sceneName, long sceneInstanceId)
        {
            CurrentScenesComponent currentScenesComponent = root.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的
            Scene currentScene = EntitySceneFactory.CreateScene(currentScenesComponent, sceneInstanceId, IdGenerater.Instance.GenerateInstanceId(),
                SceneType.Current, sceneName);
            currentScenesComponent.Scene = currentScene;

            await EventSystem.Instance.PublishAsync(currentScene, new AfterCreateCurrentSceneAddComponent());
            await EventSystem.Instance.PublishAsync(currentScene, new AfterCreateCurrentSceneLogic());
            await EventSystem.Instance.PublishAsync(currentScene, new AfterCreateCurrentSceneLogicView());
        }
    }
}