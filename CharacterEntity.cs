namespace DungeonCrawler
{
    class CharacterEntity{
        public CharacterEntity(){
            this.name = "N.";
            this.strength = 1;
            this.defense = 1;
            this.intelligence = 1;
            this.resistence = 1;
            this.hp = 1;
            this.speed = 1;
        }
        public CharacterEntity(string name, int hp, int strength, int defense, int intelligence, int resistence, int speed){
            this.name = name;
            this.strength = strength;
            this.defense = defense;
            this.intelligence = intelligence;
            this.resistence = resistence;
            this.hp = hp;
            this.speed = speed;
        }
        protected enum AttackType{
            Physical,
            Magic
        }
        protected enum AttackAttribute{
            Fire,
            Ice,
            Earth
        }
        protected AttackAttribute MyAttribute = AttackAttribute.Fire;
        protected static Random rand = new Random();
        protected string name {get;}
        protected int hp {get;}
        protected int speed {get;}
        protected int intelligence {get;}
        protected int resistence {get;}
        protected int strength {get;}
        protected int defense {get;}
        protected virtual float getRandomFloat(){
            int myrandom = rand.Next(5);
            return (0.8f + (float) myrandom/10);
        } 
        protected float BattleOther(CharacterEntity other, AttackType attacktype){
            if(attacktype == AttackType.Physical){
                return (Math.Abs(this.strength - other.defense) * this.getRandomFloat());
            }
            // no need to specify else
            return (Math.Abs(this.intelligence - this.defense) * this.getRandomFloat());
        }
        protected float Multiplier(CharacterEntity other){
            if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Ice) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Earth) return 0.5f;
            //if(this.MyAttribute == AttackAttribute.Fire && other.MyAttribute == AttackAttribute.Ice) return 1.0f;
            if(this.MyAttribute == AttackAttribute.Ice && other.MyAttribute == AttackAttribute.Earth) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Ice && other.MyAttribute == AttackAttribute.Fire) return 0.5f;
            if(this.MyAttribute == AttackAttribute.Earth && other.MyAttribute == AttackAttribute.Fire) return 2.0f;
            if(this.MyAttribute == AttackAttribute.Earth && other.MyAttribute == AttackAttribute.Ice) return 0.5f;
            return 1.0f; //return 1 if we did not find a valid condition

        }
    }
}
