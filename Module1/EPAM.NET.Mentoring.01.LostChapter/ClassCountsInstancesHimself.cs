using System.Threading;

namespace EPAM.NET.Mentoring
{
    public class ClassCountsInstancesHimself
    {
        private static int countOfInstances;

        public ClassCountsInstancesHimself()
        {
            Interlocked.Increment(ref countOfInstances);
        }

        ~ClassCountsInstancesHimself()
        {
            Interlocked.Decrement(ref countOfInstances);
        }
    }
}
