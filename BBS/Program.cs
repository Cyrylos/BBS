using System;
using System.Collections;

namespace BBS
{
    class BBS
    {
        private int p = 464679331;
        private int q = 651366587;
        private long n;
        private int seed;
        private BitArray bits = new BitArray(20000, false);

        public int P { get => p; set => p = value; }
        public int Q { get => q; set => q = value; }
        public long N { get => n; set => n = value; }
        public int Seed { get => seed; set => seed = value; }

        public BBS()
        {
            this.N = P * Q;
            Random rnd = new Random();
            int random = rnd.Next();
            while ((random % P == 0) || (random % Q == 0))
            {
                random = rnd.Next();
            }
            this.Seed = random;
        }
        public BBS(int seed)
        {
            this.N = P * Q;
            if ((seed % P == 0) || (seed % Q == 0))
            {
                throw new System.InvalidOperationException($"Seed can't be multiple of {P} or {Q}");
            }
            this.Seed = seed;
        }

        private void WriteOutputSeparator()
        {
            Console.WriteLine("\n-------------------\n");
        }

        public void GenerateBits()
        {
            Console.WriteLine("Seed:\t{0}", this.Seed);
            WriteOutputSeparator();
            long x = Seed;
            for (int i = 0; i < 20000; i++)
            {
                x = (x * x) % N;
                bool lastBit = Convert.ToBoolean(x & 1);
                this.bits[i] = lastBit;
            }
        }

        public int CountTrue()
        {
            int count = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    count++;
                }
            }
            return count;
        }

        public bool SingleBitTest()
        {
            int bitSum = this.CountTrue();
            Console.WriteLine("Count 1:\t{0}", bitSum);
            WriteOutputSeparator();
            if ((bitSum > 9725) && (bitSum < 10275))
            {
                return true;
            }
            return false;
        }

        public bool BatchTest()
        {
            var batchCount = new int[6];
            var batchSection = new int[6, 2] { { 2315, 2685 }, { 1114, 1386 }, { 527, 723 }, { 240, 384 }, { 103, 209 }, { 103, 209 } };
            int count = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    count++;
                }
                else
                {
                    if (count > 0)
                    {
                        if ((count > 0) && (count < 6))
                        {
                            batchCount[count - 1]++;
                        }
                        else
                        {
                            batchCount[5]++;
                        }
                    }
                    count = 0;
                }
            }

            Boolean resultFlag = true;

            for (int i = 0; i < batchCount.Length; i++)
            {
                count = batchCount[i];
                int lowerLimit = batchSection[i, 0];
                int upperLimit = batchSection[i, 1];
                Console.WriteLine("Series of {0}:\t{1}", i, count);
                if ((count < lowerLimit) || (count > upperLimit))
                {
                    return false;
                }
            }
            WriteOutputSeparator();
            return resultFlag;
        }

        public bool LongBatchTest()
        {
            int max = 0;
            int count = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    count++;
                }
                else
                {
                    if (count > max)
                    {
                        max = count;
                    }
                    count = 0;
                }
            }
            Console.WriteLine("Max series:\t{0}", max);
            WriteOutputSeparator();
            if (max > 25)
            {
                return false;
            }
            return true;
        }

        public bool PokerTest()
        {
            var sequentionCount = new int[16];
            for (int i = 0; i < bits.Length; i += 4)
            {
                int p0 = Convert.ToInt32(bits[i]) * 1;
                int p1 = Convert.ToInt32(bits[i + 1]) * 2;
                int p2 = Convert.ToInt32(bits[i + 2]) * 4;
                int p3 = Convert.ToInt32(bits[i + 3]) * 8;
                int sequentionValue = p0 + p1 + p2 + p3;
                sequentionCount[sequentionValue]++;
            }

            int sum = 0;
            for (int i = 0; i < sequentionCount.Length; i++)
            {
                sum += sequentionCount[i] * sequentionCount[i];
            }
            double x = (16d / 5000d) * sum - 5000d;
            Console.WriteLine("Poker sum:\t{0}", sum);
            Console.WriteLine("Poker value:\t{0}", x);
            WriteOutputSeparator();
            if ((x > 2.16) && (x < 46.17))
            {
                return true;
            }
            return false;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BBS test = new BBS();
            test.GenerateBits();
            bool singleBitFlag = test.SingleBitTest();
            bool batchFlag = test.BatchTest();
            bool longBatchFlag = test.LongBatchTest();
            bool pokerFlag = test.PokerTest();
            Console.WriteLine("Single Bit Test:\t{0}\n" +
                "Batch Test:\t\t{1}\n" +
                "Long Batch Test:\t{2}\n" +
                "Poker Test:\t\t{3}", singleBitFlag, batchFlag, longBatchFlag, pokerFlag);

        }
    }
}
