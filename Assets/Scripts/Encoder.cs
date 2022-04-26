using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Encoder {

    private int rawValue, prevValue;
    private int origin;

    public int RawValue {
        get { return rawValue; }
        internal set {
            prevValue = rawValue;
            rawValue = value;
        }
    }

    public int Value => rawValue - origin;
    public int Delta => rawValue - prevValue;

    public void ResetOrigin() {
        origin = rawValue;
    }

    [InlineProperty]
    public abstract class Event {

        public abstract void Invoke(Encoder encoder);

        public abstract class Step : Event {

            [SerializeField, HideReferenceObjectPicker, HideLabel] protected UnityEvent<int> onEvent = new UnityEvent<int>();

            public class Absolute : Step {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.Value);
                }

            }
            public class Delta : Step {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.Delta);
                }

            }
            public class Raw : Step {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.RawValue);
                }

            }

        }

        public abstract class Map : Event {

            [SerializeField] protected float stepSize = 1f;
            [SerializeField, HideReferenceObjectPicker, HideLabel] protected UnityEvent<float> onEvent = new UnityEvent<float>();


            public class Absolute : Map {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.Value * stepSize);
                }

            }
            public class Delta : Map {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.Delta * stepSize);
                }

            }
            public class Raw : Map {

                public override void Invoke(Encoder encoder) {
                    onEvent.Invoke(encoder.RawValue * stepSize);
                }

            }

        }

    }

}