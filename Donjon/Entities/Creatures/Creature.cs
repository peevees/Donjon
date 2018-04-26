﻿using System;
using Donjon.Utils;

namespace Donjon.Entities.Creatures
{
    internal abstract class Creature : IDrawable
    {
        protected int BaseDamage;

        private int health;

        protected Creature(string name, string symbol, ConsoleColor color, int health)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
            Health = MaxHealth = health;
        }

        public Position Position { get; set; }

        public int Health
        {
            get => Math.Min(health, MaxHealth);
            set => health = Math.Min(value, MaxHealth);
        }

        public int MaxHealth { get; set; }

        public bool IsDead => Health <= 0;
        public virtual int Damage => BaseDamage;


        public Level Level { get; set; }

        public Log Log { protected get; set; }
        public string Name { get; }
        public virtual ConsoleColor Color { get; }
        public string Symbol { get; }

        protected virtual bool Attack(Creature opponent)
        {
            if (opponent.IsDead) return false;
            Log.Add($"The {Name} attacks the {opponent.Name} ({opponent.Health})");
            opponent.Defend(this, Damage);
            return true;
        }

        protected virtual void Defend(Creature creature, int damage)
        {
            Health -= damage;
            var message = $"The {Name} takes {damage} hp damage from the {creature.Name}";
            if (IsDead) message += " and dies";
            Log.Add(message);
        }

        public virtual bool Walk(Position movement)
        {
            var target = Position + movement.Step;
            if (Level.IsBlockedAt(target)) return false;

            return Level.Move(Position, target);
        }
    }
}