namespace DungeonCrawler
{
    class CharacterEntity: IComparable{
        public CharacterEntity(){
            this.name = "N.";
            this.strength = 1;
            this.defense = 1;
            this.intelligence = 1;
            this.resistance = 1;
            this.hp = 1;
            this.speed = 1;
        }
        public CharacterEntity(string name, int hp, int strength, int defense, int intelligence, int resistance, int speed){
            this.name = name;
            this.strength = strength;
            this.defense = defense;
            this.intelligence = intelligence;
            this.resistance = resistance;
            this.hp = hp;
            this.speed = speed;
        }
        public enum AttackType{
            Physical,
            Magic
        }
        public enum AttackAttribute{
            Fire,
            Ice,
            Earth
        }
        protected AttackAttribute MyAttribute = AttackAttribute.Fire;
        protected static Random rand = new Random();
        public string name {get;set;}
        public int hp {get;set;}
        public int speed {get;set;}
        public int intelligence {get;set;}
        public int resistance {get;set;}
        public int strength {get;set;}
        public int defense {get;set;}
        protected virtual float getRandomFloat(){
            int myrandom = rand.Next(5);
            return (0.8f + (float) myrandom/10);
        }
        public bool TakesDamage(double damage) {
            this.hp = (int)Math.Floor(this.hp - damage);
            if(this.hp <= 0)
                return false;
            return true;
        }
        public float BattleOther(CharacterEntity other, AttackType attacktype){
            if(attacktype == AttackType.Physical){
                return (Math.Abs(this.strength - other.defense) * this.getRandomFloat());
            }
            // no need to specify else
            return (Math.Abs(this.intelligence - this.resistance) * this.getRandomFloat());
        }
        public float Multiplier(CharacterEntity other){
            if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Ice) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Earth) return 0.5f;
            //if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Ice) return 1.0f;
            if(this.MyAttribute == AttackAttribute.Ice && other.MyAttribute == AttackAttribute.Earth) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Ice && other.MyAttribute == AttackAttribute.Fire) return 0.5f;
            if(this.MyAttribute == AttackAttribute.Earth && other.MyAttribute == AttackAttribute.Fire) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Earth && other.MyAttribute == AttackAttribute.Ice) return 0.5f;
            return 1.0f; //return 1 if we did not find a valid condition

        }

        public int CompareTo(object? other)
        {
            if(other == null) return 1;
            CharacterEntity? otherCharacter = other as CharacterEntity;
            if(otherCharacter != null){
                return this.speed.CompareTo(otherCharacter.speed);
            }
            else{
                throw new ArgumentException("Other objecs it not of type CharacterEntity");
            }
        }
    }
}
