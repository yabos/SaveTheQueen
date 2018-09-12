using Aniz.NodeGraph.Level.Group;
using UnityEditor;

public interface ISubCharacterViewer
{
    void Init(EditorWindow editorWindow);

    void OnEnable();

    void OnDisable();

    void ChangeActor(ActorRoot actorRoot);

    void OnGUI();
}