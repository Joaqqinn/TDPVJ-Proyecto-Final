using Bardent.Combat.PoiseDamage;
using Bardent.ModifierSystem;

namespace Bardent.CoreSystem
{
    public class PoiseDamageReceiver : CoreComponent, IPoiseDamageable
    {
        private Stats stats;

        public Modifiers<Modifier<PoiseDamageData>, PoiseDamageData> Modifiers { get; }

        public PoiseDamageReceiver()
        {
            Modifiers = new Modifiers<Modifier<PoiseDamageData>, PoiseDamageData>();
        }

        public void DamagePoise(PoiseDamageData data)
        {
            data = Modifiers.ApplyAllModifiers(data);

            stats.Poise.Decrease(data.Amount);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}
