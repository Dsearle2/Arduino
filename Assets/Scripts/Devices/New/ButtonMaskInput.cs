using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaskInput : ArduinoInput {

    [SerializeField, InlineEditor, DisableContextMenu,
     OnValueChanged("UpdateNames"),
     ListDrawerSettings(CustomAddFunction = "@ScriptableObject.CreateInstance<ButtonInput>()")]
    private ButtonInput[] buttons;
    
    private int raw, prevRaw;

    private void UpdateNames() {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].name = i.ToString();
        }
    }

    public int Raw(int index) {
        return buttons[index].Raw;
    }

    public bool Value(int index) {
        return buttons[index].Value;
    }

    public bool Delta(int index) {
        return buttons[index].Delta;
    }

    public override void Parse(ref int index, string[] inputs) {
        if (int.TryParse(inputs[index++], out int buttonStates)) {
            prevRaw = raw;
            raw = buttonStates;
            if (prevRaw != raw) OnChangedInvoke();

            for (int i = 0; i < buttons.Length; i++) {
                buttons[i].Raw = raw & (1 << i);
            }
        }
    }

    public override T GetRaw<T>() {
        return TryCast<T>(raw);
    }
    public override T GetValue<T>() {
        return TryCast<T>(raw);
    }
    public override T GetDelta<T>() {
        return TryCast<T>(raw ^ prevRaw);
    }

    public override IEnumerable<ArduinoInput> Children {
        get {
            foreach (var button in buttons) {
                yield return button;
            }
            yield break;
        }
    }

}