using System;
using System.Collections.Generic;
using System.Linq;

namespace dot_core_asp.Models
{
    public class DicesSumModel
    {
        public int NumberOfDices => (int)Math.Pow(6, 8);

        /// <summary>
        // Let i be number of dices and j equal to the sum.
        // From the i in f(i,j) consider every dice value subtracted for i-1 , i.e.
        // Sum(f(i-1,j-l))_l<7 = f(i-1,j-1) + f(i-1,j-2) + f(i-1,j-3) + f(i-1,j-4) + f(i-1,j-5) + f(i-1,j-6)
        /// </summary>
        /// <param name="numberOfDices"></param>
        /// <param name="sumDices"></param>
        public int ProbabilityDiceSum(int numberOfDices, int sumDices)
        {
            Dictionary<(int,int), int> DicesAndSums = new Dictionary<(int,int), int>();

            var singleDiceValues = Enumerable.Range(1, 6);
            
            IEnumerable<int> sumRange = Enumerable.Range(1, numberOfDices*6);


            foreach (int diceSum in sumRange)
            {
                if (diceSum <= 6)
                    DicesAndSums[(1, diceSum)] = 1;
                else
                    DicesAndSums[(1, diceSum)] = 0;
                // Console.WriteLine(DicesAndSums[(1, diceSum)]);
            }
            
            if (numberOfDices > 1)
            {
                IEnumerable<int> diceRange = Enumerable.Range(2, numberOfDices - 1);
                foreach (int dices in diceRange)
                {
                    foreach (int diceSum in sumRange)
                    {
                        DicesAndSums[(dices, diceSum)] = 0;
                        foreach (int sumInd in singleDiceValues)
                        {
                            if (diceSum - sumInd > 0)
                                DicesAndSums[(dices, diceSum)] += DicesAndSums[(dices - 1, diceSum - sumInd)]; 
                        }
                    }
                }
            }

            Console.WriteLine(String.Format("================== Combinations for sum = {0} with {1} dices ==================", sumDices, numberOfDices));
            if (sumDices <= numberOfDices*6)
                Console.WriteLine(DicesAndSums[(numberOfDices, sumDices)]);
            else
                Console.WriteLine(0);
            Console.WriteLine(String.Format("================== Probability for sum = {0} with {1} dices ==================", sumDices, numberOfDices));
            if (sumDices <= numberOfDices*6)
                Console.WriteLine((double)DicesAndSums[(numberOfDices, sumDices)]/((int)Math.Pow(6, numberOfDices)));
            else
                Console.WriteLine(0); 

            return DicesAndSums[(numberOfDices, sumDices)];
        }

        public void TestProbabilityDiceSum()
        {
            int dices = 8;
            int allCombinations = (int)Math.Pow(6, dices);
            Console.WriteLine(String.Format("All combinations {0}", allCombinations));

            // All ones
            int combinationsAllOnes = ProbabilityDiceSum(dices, dices);
            Console.WriteLine(String.Format("All one test is {0}", combinationsAllOnes == 1));

            // All 6s
            int combinationsAllSix = ProbabilityDiceSum(dices, dices*6);
            Console.WriteLine(String.Format("All sixes test is {0}", combinationsAllSix == 1));

            Console.WriteLine(String.Format("All six and all one give same result {0}", combinationsAllOnes == combinationsAllSix));

            // One 2 and otherwise all ones
            int combinationsATwoAndOnes = ProbabilityDiceSum(dices, dices + 1);
            Console.WriteLine(String.Format("All one test is {0}", combinationsATwoAndOnes == dices));

            // A 5 and otherwise all 6s
            int combinationsAFiveAndAllSix = ProbabilityDiceSum(dices, dices*6 - 1);
            Console.WriteLine(String.Format("All sixes test is {0}", combinationsAFiveAndAllSix == dices));

            Console.WriteLine(String.Format("Consistent result for one add/subtracted to sum {0}", combinationsATwoAndOnes == combinationsAFiveAndAllSix));
        }


    }
}