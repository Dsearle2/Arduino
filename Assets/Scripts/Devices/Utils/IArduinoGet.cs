using Sirenix.OdinInspector;
using System;

[HideLabel]
public interface IArduinoGet {

    public abstract T Get<T>(ArduinoInput input);
    
}

namespace ArduinoGet {

    [Serializable]
    public class Value : IArduinoGet {
        public T Get<T>(ArduinoInput input) => input.GetValue<T>();
    }

    [Serializable]
    public class Delta : IArduinoGet {
        public T Get<T>(ArduinoInput input) => input.GetDelta<T>();
    }

    [Serializable]
    public class Raw : IArduinoGet {
        public T Get<T>(ArduinoInput input) => input.GetRaw<T>();
    }
    
}