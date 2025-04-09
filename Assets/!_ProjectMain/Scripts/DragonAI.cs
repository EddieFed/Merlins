using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;
using UnityEngine;

namespace __ProjectMain.Scripts
{
    class DragonAI
    {
        private InferenceEngine engine = new InferenceEngine();

        [System.Serializable]
        public enum Distance
        {
            Near = 1,
            Medium = 2,
            Far = 3
        }; // 1: Near, 2: Medium, 3: Far

        public enum DragonAction
        {
            BreatheFire = 0,
            RunAway = 1,
            Shop = 2,
            StandStill = 3
        };
        
        private Distance currentDistance = Distance.Near;

        public DragonAction GetDragonAction(Distance d)
        {
            currentDistance = d;
            int distanceObservation = (int)currentDistance;
            
            Variable<int> distance = Variable.Observed(distanceObservation).Named("distance");
            Discrete actionProb = distanceObservation switch
            {
                1 => new Discrete(new double[] { 0.8, 0.05, 0.1, 0.05 }), // Near: high chance to BreatheFire
                2 => new Discrete(new double[] { 0.5, 0.2, 0.2, 0.1 }), // Medium: more balanced
                3 => new Discrete(new double[] { 0.25, 0.25, 0.25, 0.25 }), // Far: equal chances
                _ => new Discrete(new double[] { 0.25, 0.25, 0.25, 0.25 }) // Default case (Far)
            };
            
            Variable<int> action = Variable.New<int>().Named("action");
            action.SetTo(Variable.Random(actionProb));
            
            Discrete result = engine.Infer<Discrete>(action);
            return (DragonAction) result.Sample();
        }
    }
}

// The following did not work, keep this here for reference
// - Eddie

/*
public class DragonAI : MonoBehaviour
{
    private Range customerCountRange;   // 1: 1, 2: 2, 3: 3+
    private Range speedRange;           // 1: Walking, 2: Running
    private Range distanceRange;        // 1: Close, 2: Near, 3: Far
    private Range actionRange;          // 1: Breathe Fire, 2: Run Away, 3: Shop, 4: Stand Still
    //            ^ Resulting action

    private Variable<int> customerCount;
    private Variable<int> entranceDistance;
    private Variable<int> speed;

    [SerializeField, Range(1, 3)] public int LIVE_CustomerCount;
    [SerializeField, Range(1, 3)] public int LIVE_EntranceDistance;
    [SerializeField, Range(1, 2)] public int LIVE_Speed;

    // ReSharper disable once InconsistentNaming
    VariableArray3D<VariableArray<Discrete>> actionCPT = null;

    private void Start()
    {
        // Init table!!!!
        customerCountRange = new Range(3);
        distanceRange = new Range(3);
        speedRange = new Range(2);
        actionRange = new Range(4);

        // Default init, these can be updated in Update()
        customerCount = Variable.New<int>().Named("customerCount");
        customerCount.SetValueRange(customerCountRange);
        customerCount.ObservedValue = 1;
        entranceDistance = Variable.New<int>().Named("entranceDistance");
        entranceDistance.SetValueRange(distanceRange);
        entranceDistance.ObservedValue = 1;
        speed = Variable.New<int>().Named("speed");
        speed.SetValueRange(speedRange);
        speed.ObservedValue = 1;

        actionCPT = Variable.Array<VariableArray<Discrete>>(customerCountRange, distanceRange, speedRange).Named("ActionCPT");

        // Let's define probabilities for each action based on the entrance distance
        double[] prob = entranceDistance.ObservedValue switch
        {
            1 => new double[] { 0.7, 0.1, 0.1, 0.1 },  // Close: 70% chance to breathe fire
            2 => new double[] { 0.4, 0.3, 0.2, 0.1 },  // Near: 40% chance to breathe fire
            3 => new double[] { 0.1, 0.5, 0.2, 0.2 },  // Far: 10% chance to breathe fire
            _ => new double[] { 0.25, 0.25, 0.25, 0.25 }  // Default evenly spread if no valid distance
        };

        // Set the action's distribution based on the probabilities
        Discrete actionDist = new Discrete(prob);
        actionCPT.SetTo(Variable.Observed(actionDist));  // Observed action based on the distance
    }

    // // ReSharper disable once InconsistentNaming
    // private void BuildCPT()
    // {
    //     // Build CPT with probabilities. Ideally this would be loaded from a csv or something
    //     // For now I'm just using a dummy data loop to create it...
    //     Discrete[,] rawCPT = new Discrete[3, 3];
    //     for (int c = 0; c < 3; c++)
    //     {
    //         for (int d = 0; d < 3; d++)
    //         {
    //             for (int s = 0; s < 2; s++)
    //             {
    //                 double[] prob = c switch
    //                 {
    //                     // Thanks rider, i sure do love pattern matching lol
    //                     2 when d == 2 && s == 1 => new double[] { 0.7, 0.2, 0.05, 0.05 },
    //                     0 when d == 0 && s == 1 => new double[] { 0.05, 0.1, 0.8, 0.05 },
    //                     2 when d == 0 && s == 1 => new double[] { 0.2, 0.7, 0.05, 0.05 },
    //                     _ => new double[] { 0.25, 0.25, 0.25, 0.25 }
    //                 };
    //
    //                 rawCPT[c, d] = new Discrete(prob);
    //             }
    //         }
    //     }
    //     actionCPT.ObservedValue = rawCPT;
    // }

    private void GetDragonAction()
    {
        Variable<int> action = Variable.New<int>().Named("Action");
        action.SetValueRange(actionRange);

        // Use Switch with the probabilistic variables `c`, `d`, and `s`
        using (Variable.Switch(customerCount))
        using (Variable.Switch(entranceDistance))
        using (Variable.Switch(speed))
        {
            action.SetTo(Variable.Random<int, Discrete>(actionCPT[customerCount, entranceDistance, speed]));
        }

        // Run inference
        InferenceEngine engine = new InferenceEngine();
        Discrete res = engine.Infer<Discrete>(action);

        string[] actionLabels = { "BreatheFire", "RunAway", "Shop", "Stay" };
        Debug.Log("Action probabilities:");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"{actionLabels[i]}: {res[i]:F2}");
        }
    }

    private void Update()
    {
        // Just check current status on spacebar press for now
        if (Input.GetKeyDown(KeyCode.Space))
        {
            customerCount.ObservedValue = LIVE_CustomerCount;
            entranceDistance.ObservedValue = LIVE_EntranceDistance;
            speed.ObservedValue = LIVE_Speed;
            GetDragonAction();
        }
    }
}
*/