using Bogus;

namespace RedisApp.WebApi.Domain.Helpers;

public sealed class PrivateFaker<T> : Faker<T> where T : class
{
    public PrivateFaker<T> UsePrivateConstructor()
    {
        return CustomInstantiator(f => Activator.CreateInstance(typeof(T), nonPublic: true) as T)
           as PrivateFaker<T>;
    }

    public PrivateFaker<T> RuleForPrivate<TProperty>(string propertyName, Func<Faker, TProperty> setter)
    {
        base.RuleFor(propertyName, setter);
        return this;
    }
}
