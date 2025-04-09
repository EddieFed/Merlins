using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;
using UnityEngine;

namespace __ProjectMain.Scripts
{
    public class DragonAI : MonoBehaviour
    {
        private Range customerCountRange;   // 1: 1, 2: 2, 3: 3+
        private Range speedRange;           // 1: Walking, 2: Running
        private Range distanceRange;        // 1: Close, 2: Near, 3: Far
        private Range actionRange;          // 1: Breathe Fire, 2: Run Away, 3: Shop, 4: Stand Still
        //            ^ Resulting action
        
        public Variable<int> CustomerCount;
        public Variable<int> Speed;
        public Variable<int> EntranceDistance;

        // ReSharper disable once InconsistentNaming
        private VariableArray3D<Discrete> actionCPT = null;
        
        private void Start()
        {
            // Init table!!!!
            customerCountRange = new Range(3);
            speedRange = new Range(3);
            distanceRange = new Range(3);
            actionRange = new Range(4);
            
            // Default init, these can be updated in Update()
            CustomerCount = Variable.Observed(1);
            EntranceDistance = Variable.Observed(1);
            Speed = Variable.Observed(1);
            
            actionCPT = Variable.Array<Discrete>(customerCountRange, distanceRange, speedRange).Named("ActionCPT");
            BuildCPT();
        }

        // ReSharper disable once InconsistentNaming
        private void BuildCPT()
        {
            // Build CPT with probabilities. Ideally this would be loaded from a csv or something
            // For now I'm just using a dummy data loop to create it...
            for (int c = 0; c < 3; c++)
            {
                for (int d = 0; d < 3; d++)
                {
                    for (int s = 0; s < 3; s++)
                    {
                        double[] prob = c switch
                        {
                            // Thanks rider, i sure do love pattern matching lol
                            2 when d == 2 && s == 2 => new double[] { 0.7, 0.2, 0.05, 0.05 },
                            0 when d == 0 && s == 1 => new double[] { 0.05, 0.1, 0.8, 0.05 },
                            2 when d == 0 && s == 2 => new double[] { 0.2, 0.7, 0.05, 0.05 },
                            _ => new double[] { 0.25, 0.25, 0.25, 0.25 }
                        };

                        Discrete actionDist = new Discrete(prob);
                        actionCPT[Variable.Discrete(c), Variable.Discrete(d), Variable.Discrete(s)].SetTo(Variable.Observed(actionDist));
                    }
                }
            }
        }
        
        public void GetDragonAction()
        {
            Variable<int> action = Variable.New<int>().Named("Action");
        
            // Use Switch with the probabilistic variables `c`, `d`, and `s`
            using (Variable.Switch(CustomerCount))
            using (Variable.Switch(EntranceDistance))
            using (Variable.Switch(Speed))
            {
                action.SetTo(Variable.Random<int, Discrete>(actionCPT[CustomerCount, EntranceDistance, Speed]));
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
    }
}