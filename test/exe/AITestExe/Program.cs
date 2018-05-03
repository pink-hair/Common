using System;

namespace AITestExe
{
    using Polytech.Common.Telemetron;
    using Polytech.Common.Telemetron.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            ApplicationInsightsTelemetronConfiguration aitc = new ApplicationInsightsTelemetronConfiguration()
            {
                InstrumentationKey = "78226715-2ae2-48be-b168-3ec68a10ddcc"
            };

            ApplicationInsightsTelemetron tel = new ApplicationInsightsTelemetron(aitc);

            for (int i = 0; i < 10; i++)
            {
                using (var op1 = tel.CreateOperation("op1"))
                {
                    tel.Info("foo1", "p1tmtdMt00M");

                    using (var op2 = tel.CreateOperation("op2"))
                    {
                        tel.Info("foo2", "p1tmtdMt00M");

                        using (var op3 = tel.CreateOperation("op3"))
                        {
                            tel.Info("foo3", "p1tmtdMt00M");

                            using (var op4 = tel.CreateOperation("op4"))
                            {
                                tel.Info("foo4", "p1tmtdMt00M");
                                tel.Metric("met", 3);
                                tel.Event("evt");
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
