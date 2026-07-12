using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories
{
    public class AbstractOverAsyncFactory<TFactory, TValue> : IAbstractOverAsyncFactory<TValue>
        where TFactory : IFactory<UniTask<TValue>>
    {
        private readonly TFactory _factory;

        public TValue CreatedValue { get; private set; }

        public AbstractOverAsyncFactory(TFactory factory) => 
            _factory = factory;

        public async UniTask CreateAsync() => 
            CreatedValue = await _factory.Create();
    }

    public interface IAbstractOverAsyncFactory<out TValue>
    {
        public TValue CreatedValue { get; }
        
        public UniTask CreateAsync();
    }
}