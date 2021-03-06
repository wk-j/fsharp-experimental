
using System.Threading.Tasks;

public class C<T> where T: struct
{
    public delegate T OnDemand();

    public static Nullable<T> CreateSimleNullable(T item) 
    {
        return new Nullable<T>(item);
    }
    public static OnDemand CreateSimpleOnDemand(T item)
    {
        return () => item;
    }
    public static IEnumerable<T> CreateSimpleSequence(T item)
    {
        yield return item;
    }
}

public class Main {

    /* 
    public static Nullable<int> ApplyFunction(
        Nullable<int> nullable,
        Func<int,int> function) {
            if(nullable.HasValue) {
                var u = nullable.Value;
                var r = function(u);
                return new Nullable<int>(r);
            } else {
                return new Nullable<int>();
            }
        }
    */

    public static Nullable<T> ApplyFunction<T>( Nullable<T> nullable, Func<T, T> function) where T: struct {
        if (nullable.HasValue) {
            T unwrapped = nullable.Value;
            T result = function(unwrapped);
            return new Nullable<T>(result);
        }
        else
            return new Nullable<T>();
    }

    public static IEnumerable<int> AddOne2(IEnumerable<int> sequence) => from item in sequence select item + 1;

    public static IEnumerable<int> AddOne(IEnumerable<int> sequence) {
        foreach(var u in sequence) {
            var v = u + 1;
            yield return v;
        }
    }

    public static C<int>.OnDemand AddOne(C<int>.OnDemand onDemand) {
        return () => {
            var unwrapped = onDemand();
            var result = unwrapped + 1;
            return result;
            //return C<int>.CreateSimpleOnDemand(result);
        };
    }

    public static Lazy<int> AddOne(Lazy<int> lazy) {
        return new Lazy<int>(() => {
            var u = lazy.Value;
            var v = u + 1;
            return v;
        });
    }

    public async Task<int> AddOne(Task<int> task){
        var u = await task;
        var v = u + 1;
        return v;
    }

    public static Nullable<int> AddOne(Nullable<int> nullable)
    {
        return ApplyFunction(nullable, (x) => x + 1);
    }
}

var x = Main.AddOne(new Nullable<int>());

Console.WriteLine(x);
