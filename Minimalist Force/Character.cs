using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame2
{
    internal class Character
    {
        public const int OFFSETX = 0;
        public const int OFFSETY = 0;
        public const int MAXNUMBER_ITEMS = 4;
        public const int MAXNUMBER_WEAPONS = 4;
        public const int MAXNUMBER_RINGS = 4;
        public const int MAXNUMBER_SPELLS = 4;
        public const int LEVELATWHICHPROMOTED_DEFAULT = 20;
        public const int LEVELATWHICHPROMOTED_ELRIC = 21;
        public const int LEVELATWHICHPROMOTED_KARNA = 24;
        public const int LEVELATWHICHPROMOTED_JANET = 24;
        public const int LEVELATWHICHPROMOTED_ERIC = 24;
        public const int LEVELATWHICHPROMOTED_RANDOLF = 24;
        public const int LEVELATWHICHPROMOTED_TYRIN = 24;

        public enum Enemies
        {
            Ninja,
            Banana,
            Witch,
            Samurai,
            Tower
        }

        public static Texture2D TexturesJacob;
        public static Texture2D TexturesCaryn;
        public static Texture2D TexturesTassi;
        public static Texture2D TexturesEva;
        public static Texture2D TexturesHans;
        public static Texture2D TexturesMonica;
        public static Texture2D TexturesNeutralTassi;
        public static Texture2D TexturesNeutralEva;
        public static Texture2D TexturesNeutralMonica;

        public static Texture2D TexturesNinja;
        public static Texture2D TexturesBanana;
        public static Texture2D TexturesWitch;
        public static Texture2D TexturesSamurai;

        public static Texture2D TexturesNeutralTower;
        public static Texture2D TexturesPartyTower;
        public static Texture2D TexturesEnemiesTower;

        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _level;

        public int Level
        {
            get { return _level; }
            set { while (_level < value) NextLevel(); }
        }

        private int _hitPoints;

        public int HitPoints
        {
            get { return _hitPoints; }
            set { _hitPoints = value; }
        }

        private int _maxHitPoints;

        public int MaxHitPoints
        {
            get { return _maxHitPoints; }
            set { _maxHitPoints = value; }
        }

        private int _attackPoints;

        public int AttackPoints
        {
            get { return _attackPoints; }
            set { _attackPoints = value; }
        }

        private int _defensePoints;

        public int DefensePoints
        {
            get { return _defensePoints; }
            set { _defensePoints = value; }
        }

        private int _magicPoints;

        public int MagicPoints
        {
            get { return _magicPoints; }
            set { _magicPoints = value; }
        }

        private int _maxMagicPoints;

        public int MaxMagicPoints
        {
            get { return _maxMagicPoints; }
            set { _maxMagicPoints = value; }
        }

        private bool _mustSurvive;

        public bool MustSurvive
        {
            get { return _mustSurvive; }
            set { _mustSurvive = value; }
        }

        private int _movePoints;

        public int MovePoints
        {
            get { return _movePoints; }
            set { _movePoints = value; }
        }

        private int _agility;

        public int Agility
        {
            get { return _agility; }
            set { _agility = value; }
        }

        private bool _flying;

        public bool Flying
        {
            get { return _flying; }
            set { _flying = value; }
        }

        private Texture2D _texture;

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private float _position;

        public float Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private bool _alive;

        public bool Alive
        {
            get { return _alive; }
            set { _alive = value; }
        }

        private bool _visible;

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        private int _experiencePoints;

        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set { _experiencePoints = value; }
        }

        private int _minAttackRange;

        public int MinAttackRange
        {
            get { return _minAttackRange; }
            set { _minAttackRange = value; }
        }

        private int _maxAttackRange;

        public int MaxAttackRange
        {
            get { return _maxAttackRange; }
            set { _maxAttackRange = value; }
        }

        public Item[] Items = new Item[MAXNUMBER_ITEMS];

        public bool[] ItemEquipped = new bool[MAXNUMBER_ITEMS];

        public Spell[] MagicSpells = new Spell[MAXNUMBER_SPELLS];

        private int _asleep;

        public int Asleep
        {
            get { return _asleep; }
            set { _asleep = value; }
        }

        private bool _poisoned;

        public bool Poisoned
        {
            get { return _poisoned; }
            set { _poisoned = value; }
        }

        private bool _cursed;

        public bool Cursed
        {
            get { return _cursed; }
            set { _cursed = value; }
        }

        private int _stunned;

        public int Stunned
        {
            get { return _stunned; }
            set { _stunned = value; }
        }

        private int _silenced;

        public int Silenced
        {
            get { return _silenced; }
            set { _silenced = value; }
        }

        private int _confused;

        public int Confused
        {
            get { return _confused; }
            set { _confused = value; }
        }

        private int _oldMaxHitPoints;

        public int OldMaxHitPoints
        {
            get { return _oldMaxHitPoints; }
            set { _oldMaxHitPoints = value; }
        }

        private int _oldMaxMagicPoints;

        public int OldMaxMagicPoints
        {
            get { return _oldMaxMagicPoints; }
            set { _oldMaxMagicPoints = value; }
        }

        private int _oldAgility;

        public int OldAgility
        {
            get { return _oldAgility; }
            set { _oldAgility = value; }
        }

        private int _oldAttackPoints;

        public int OldAttackPoints
        {
            get { return _oldAttackPoints; }
            set { _oldAttackPoints = value; }
        }

        private int _oldDefensePoints;

        public int OldDefensePoints
        {
            get { return _oldDefensePoints; }
            set { _oldDefensePoints = value; }
        }

        private int _levelToDisplay;

        public int LevelToDisplay
        {
            get { return _levelToDisplay; }
            set { _levelToDisplay = value; }
        }

        private bool _susceptibleFire;

        public bool SusceptibleFire
        {
            get { return _susceptibleFire; }
            set { _susceptibleFire = value; }
        }

        private bool _susceptibleIce;

        public bool SusceptibleIce
        {
            get { return _susceptibleIce; }
            set { _susceptibleIce = value; }
        }

        private bool _canPoison;

        public bool CanPoison
        {
            get { return _canPoison; }
            set { _canPoison = value; }
        }

        private bool _canSlow;

        public bool CanSlow
        {
            get { return _canSlow; }
            set { _canSlow = value; }
        }

        private bool _canStun;

        public bool CanStun
        {
            get { return _canStun; }
            set { _canStun = value; }
        }

        private bool _canMuddle;

        public bool CanMuddle
        {
            get { return _canMuddle; }
            set { _canMuddle = value; }
        }

        private bool _canDispel;

        public bool CanDispel
        {
            get { return _canDispel; }
            set { _canDispel = value; }
        }

        private bool _canSleep;

        public bool CanSleep
        {
            get { return _canSleep; }
            set { _canSleep = value; }
        }

        private bool _resistantFire;

        public bool ResistantFire
        {
            get { return _resistantFire; }
            set { _resistantFire = value; }
        }

        private bool _resistantIce;

        public bool ResistantIce
        {
            get { return _resistantIce; }
            set { _resistantIce = value; }
        }

        private bool _boss;

        public bool Boss
        {
            get { return _boss; }
            set { _boss = value; }
        }

        private int _boosted;

        public int Boosted
        {
            get { return _boosted; }
            set { _boosted = value; }
        }

        private int _attackBoosted;

        public int AttackBoosted
        {
            get { return _attackBoosted; }
            set { _attackBoosted = value; }
        }

        private int _agilityBoostedBy;

        public int AgilityBoostedBy
        {
            get { return _agilityBoostedBy; }
            set { _agilityBoostedBy = value; }
        }

        private int _defenseBoostedBy;

        public int DefenseBoostedBy
        {
            get { return _defenseBoostedBy; }
            set { _defenseBoostedBy = value; }
        }

        private int _attackBoostedBy;

        public int AttackBoostedBy
        {
            get { return _attackBoostedBy; }
            set { _attackBoostedBy = value; }
        }

        private bool _hasSpecialAttack;

        public bool HasSpecialAttack
        {
            get { return _hasSpecialAttack; }
            set { _hasSpecialAttack = value; }
        }

        // constructor for player party members
        public Character(String name)
        {
            switch (name)
            {
                case "HERO":
                    Init("HERO", 1, 1, 12, 12, 6, 4, 8, 8, true, 6, 4, false, true, 1, 1, false, null, new Spell[] { new Spell("RECRUIT", 1), null, null, null }, TexturesJacob);
                    break;

                case "HEALER":
                    Init("HEALER", 1, 1, 11, 11, 6, 5, 10, 10, false, 5, 5, false, true, 1, 1, false, null, new Spell[] { new Spell("HEAL", 1), null, null, null }, TexturesCaryn);
                    break;

                case "WARRIOR":
                    Init("WARRIOR", 1, 1, 9, 9, 9, 7, 0, 0, false, 5, 4, false, true, 1, 1, false, null, null, TexturesTassi);
                    break;

                case "ARCHER":
                    Init("ARCHER", 1, 1, 8, 8, 7, 6, 0, 0, false, 5, 4, false, true, 2, 3, false, null, null, TexturesEva);
                    break;

                case "WIZARD":
                    Init("WIZARD", 1, 1, 8, 8, 6, 4, 20, 20, false, 6, 3, false, true, 1, 1, false, null, new Spell[] { new Spell("BLAZE", 2), null, null, null }, TexturesMonica);
                    break;

                case "N.WARRIOR":
                    Init("WARRIOR", 1, 1, 9, 9, 9, 7, 0, 0, false, 5, 4, false, true, 1, 1, false, null, null, TexturesNeutralTassi);
                    break;

                case "N.ARCHER":
                    Init("ARCHER", 1, 1, 8, 8, 7, 6, 0, 0, false, 5, 4, false, true, 2, 3, false, null, null, TexturesNeutralEva);
                    break;

                case "N.WIZARD":
                    Init("WIZARD", 1, 1, 8, 8, 6, 4, 20, 20, false, 6, 3, false, true, 1, 1, false, null, new Spell[] { new Spell("BLAZE", 2), null, null, null }, TexturesNeutralMonica);
                    break;

                case "TOWER":
                    Init("TOWER", 1, 1, 60, 60, 2, 2, 0, 0, false, 0, 2, false, true, 0, 0, false, null, null, TexturesPartyTower);
                    break;

                case "N.TOWER":
                    Init("TOWER", 1, 1, 60, 60, 2, 2, 0, 0, false, 0, 2, false, true, 0, 0, false, null, null, TexturesNeutralTower);
                    break;
            }

            _hitPoints = _maxHitPoints;
            _magicPoints = _maxMagicPoints;
        }

        public void Init(String name, int level, int levelToDisplay, int hitPoints, int maxHitPoints, int attackPoints, int defensePoints, int magicPoints, int maxMagicPoints, bool mustSurvive, int movePoints, int agility, bool flying, bool alive, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            Init(name, level, levelToDisplay, hitPoints, maxHitPoints, attackPoints, defensePoints, magicPoints, maxMagicPoints, mustSurvive, movePoints, agility, flying, alive, 1, 1, hasSpecialAttack, items, magicSpells, texture);
        }

        // *** constructor temporarily for enemy party members, later on also for player party members
        public Character(int id, CharacterPointer.Sides side)
        {
            bool[] noItemEquipped = new bool[4] { false, false, false, false };

            switch (side)
            {
                case CharacterPointer.Sides.CPU_Opponents:
                    switch ((Enemies)id)
                    {
                        case Enemies.Ninja:
                            Init("NINJA", 4, 12, 15, 5 / 2, 0, false, false, 5, 23, false, 2, 3, false, false, false, false, false, false, false, false, false, false, false, new Item[] { new Item("Medical", " Herb"), null, null, null }, null, TexturesNinja);
                            break;

                        case Enemies.Banana:
                            Init("BANANA", 2, 10, 12, 3 / 2, 0, false, false, 6, 18, false, 1, 1, false, false, false, false, false, false, false, false, false, false, false, null, null, TexturesBanana);
                            break;

                        case Enemies.Witch:
                            Init("WITCH", 5, 20, 8, 2 / 2, 12, false, false, 6, 25, false, 1, 1, false, false, false, false, false, false, false, false, false, false, false, null, new Spell[] { new Spell("BLAZE", 2), null, null, null }, TexturesWitch);
                            break;

                        case Enemies.Samurai:
                            Init("SAMURAI", 6, 24, 21, 7 / 2, 12, false, false, 5, 30, false, 2, 3, false, false, false, false, false, false, false, false, false, false, false, null, new Spell[] { new Spell("RECRUIT", 1), null, null, null }, TexturesSamurai);
                            break;

                        case Enemies.Tower:
                            Init("TOWER", 1, 1, 40, 40, 2, 2, 0, 0, false, 0, 2, false, true, 0, 0, false, null, null, TexturesEnemiesTower);
                            break;
                    }
                    // ***
                    break;

                case CharacterPointer.Sides.Player:
                    // ***
                    break;
            }
        }

        // init for enemy party members
        public void Init(String name, int level, int maxHitPoints, int attackPoints, int defensePoints, int maxMagicPoints, bool mustSurvive, bool boss, int movePoints, int agility, bool flying, int minAttackRange, int maxAttackRange, bool susceptibleFire, bool susceptibleIce, bool resistantFire, bool resistantIce, bool canPoison, bool canDispel, bool canMuddle, bool canSlow, bool canStun, bool canSleep, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            Init(name, level, level, maxHitPoints, maxHitPoints, attackPoints, defensePoints, maxMagicPoints, maxMagicPoints, mustSurvive, boss, movePoints, agility, flying, true, minAttackRange, maxAttackRange, susceptibleFire, susceptibleIce, resistantFire, resistantIce, canPoison, canDispel, canMuddle, canSlow, canStun, canSleep, hasSpecialAttack, items, magicSpells, texture);
        }

        // general constructor
        public Character(String name, int level, int levelToDisplay, int hitPoints, int maxHitPoints, int attackPoints, int defensePoints, int magicPoints, int maxMagicPoints, bool mustSurvive, bool boss, int movePoints, int agility, bool flying, bool alive, int minAttackRange, int maxAttackRange, bool susceptibleFire, bool susceptibleIce, bool resistantFire, bool resistantIce, bool canPoison, bool canDispel, bool canMuddle, bool canSlow, bool canStun, bool canSleep, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            Init(name, level, levelToDisplay, hitPoints, maxHitPoints, attackPoints, defensePoints, magicPoints, maxMagicPoints, mustSurvive, boss, movePoints, agility, flying, alive, minAttackRange, maxAttackRange, susceptibleFire, susceptibleIce, resistantFire, resistantIce, canPoison, canDispel, canMuddle, canSlow, canStun, canSleep, hasSpecialAttack, items, magicSpells, texture);
        }

        public void Init(String name, int level, int levelToDisplay, int hitPoints, int maxHitPoints, int attackPoints, int defensePoints, int magicPoints, int maxMagicPoints, bool mustSurvive, bool boss, int movePoints, int agility, bool flying, bool alive, int minAttackRange, int maxAttackRange, bool susceptibleFire, bool susceptibleIce, bool resistantFire, bool resistantIce, bool canPoison, bool canDispel, bool canMuddle, bool canSlow, bool canStun, bool canSleep, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            _name = name;
            _level = level;
            _levelToDisplay = levelToDisplay;
            _hitPoints = hitPoints;
            _maxHitPoints = maxHitPoints;
            _attackPoints = attackPoints;
            _defensePoints = defensePoints;
            _magicPoints = magicPoints;
            _maxMagicPoints = maxMagicPoints;
            _mustSurvive = mustSurvive;
            _boss = boss;
            _movePoints = movePoints;
            _agility = agility;
            _flying = flying;
            _alive = alive;
            _visible = true;
            _experiencePoints = 0;
            _minAttackRange = minAttackRange;
            _maxAttackRange = maxAttackRange;
            _susceptibleFire = susceptibleFire;
            _susceptibleIce = susceptibleIce;
            _resistantFire = resistantFire;
            _resistantIce = resistantIce;
            _canPoison = canPoison;
            _canDispel = canDispel;
            _canMuddle = canMuddle;
            _canSlow = canSlow;
            _canStun = canStun;
            _canSleep = canSleep;
            _hasSpecialAttack = hasSpecialAttack;
            _asleep = 0;
            _poisoned = false;
            _cursed = false;
            _stunned = 0;
            _silenced = 0;
            _confused = 0;
            _boosted = 0;
            _attackBoosted = 0;
            _agilityBoostedBy = 0;
            _defenseBoostedBy = 0;
            _attackBoostedBy = 0;
            Items = items;
            MagicSpells = magicSpells;

            _texture = texture;
        }

        // constructor for player party members
        public Character(String name, int level, int levelToDisplay, int hitPoints, int maxHitPoints, int attackPoints, int defensePoints, int magicPoints, int maxMagicPoints, bool mustSurvive, int movePoints, int agility, bool flying, bool alive, int minAttackRange, int maxAttackRange, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            Init(name, level, levelToDisplay, hitPoints, maxHitPoints, attackPoints, defensePoints, magicPoints, maxMagicPoints, mustSurvive, movePoints, agility, flying, alive, minAttackRange, maxAttackRange, hasSpecialAttack, items, magicSpells, texture);
        }

        public void Init(String name, int level, int levelToDisplay, int hitPoints, int maxHitPoints, int attackPoints, int defensePoints, int magicPoints, int maxMagicPoints, bool mustSurvive, int movePoints, int agility, bool flying, bool alive, int minAttackRange, int maxAttackRange, bool hasSpecialAttack, Item[] items, Spell[] magicSpells, Texture2D texture)
        {
            _name = name;
            _level = level;
            _levelToDisplay = levelToDisplay;
            _hitPoints = hitPoints;
            _maxHitPoints = maxHitPoints;
            _attackPoints = attackPoints;
            _defensePoints = defensePoints;
            _magicPoints = magicPoints;
            _maxMagicPoints = maxMagicPoints;
            _mustSurvive = mustSurvive;
            _boss = false;
            _movePoints = movePoints;
            _agility = agility;
            _flying = flying;
            _alive = alive;
            _visible = true;
            _experiencePoints = 0;
            _minAttackRange = minAttackRange;
            _maxAttackRange = maxAttackRange;
            _susceptibleFire = false;
            _susceptibleIce = false;
            _resistantFire = false;
            _resistantIce = false;
            _canPoison = false;
            _canDispel = false;
            _canMuddle = false;
            _canSlow = false;
            _canStun = false;
            _canSleep = false;
            _asleep = 0;
            _poisoned = false;
            _cursed = false;
            _stunned = 0;
            _silenced = 0;
            _confused = 0;
            _boosted = 0;
            _attackBoosted = 0;
            _agilityBoostedBy = 0;
            _defenseBoostedBy = 0;
            _attackBoostedBy = 0;
            _hasSpecialAttack = hasSpecialAttack;
            Items = items;
            MagicSpells = magicSpells;

            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawAt(spriteBatch, position * new Vector2(Map.TILESIZEX, Map.TILESIZEY) + new Vector2(OFFSETX, OFFSETY), _visible);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, byte opacity1)
        {
            DrawAt(spriteBatch, position * new Vector2(Map.TILESIZEX, Map.TILESIZEY) + new Vector2(OFFSETX, OFFSETY), _visible, opacity1);
        }

        public void DrawAt(SpriteBatch spriteBatch, Vector2 posXY)
        {
            DrawAt(spriteBatch, posXY, _visible);
        }

        public void DrawAt(SpriteBatch spriteBatch, Vector2 posXY, bool visible)
        {
            DrawAt(spriteBatch, posXY, visible, 255);
        }

        public void DrawAt(SpriteBatch spriteBatch, Vector2 posXY, bool visible, byte opacity1)
        {
            if (visible)
                spriteBatch.Draw(_texture, posXY, new Color(opacity1, opacity1, opacity1, opacity1));
        }

        public int NextSpell()
        {
            int i;

            for (i = 0; i < MAXNUMBER_SPELLS; i++)
                if (MagicSpells[i] == null)
                    return i;

            return -1; // no free spell slot
        }

        public LevelUpMessage NextLevel()
        {
            LevelUpMessage returnValue = new LevelUpMessage(LevelUpMessage.Messages.None, 0);

            _level++;
            _levelToDisplay++;
            _oldMaxHitPoints = _maxHitPoints;
            _oldMaxMagicPoints = _maxMagicPoints;
            _oldAttackPoints = _attackPoints;
            _oldDefensePoints = _defensePoints;
            _oldAgility = _agility;

            // *** here comes the code that determines what skills are improved for each character and for each level
            switch (_name)
            {
                case "HERO":
                    _maxHitPoints += 1;
                    if (_level % 2 == 0) _maxMagicPoints += 1;
                    _attackPoints += 1;
                    _defensePoints += 1;
                    _agility += 1;
                    break;

                case "HEALER":
                    if (_level % 2 == 0) _maxHitPoints += 1;
                    _maxMagicPoints += 1;
                    if (_level % 2 == 0) _attackPoints += 1;
                    if (_level % 2 == 0) _defensePoints += 1;
                    _agility += 1;
                    break;

                case "WARRIOR":
                    _maxHitPoints += 1;
                    _maxMagicPoints += 0;
                    _attackPoints += 1;
                    if (_level % 2 == 0) _attackPoints += 1;
                    _defensePoints += 1;
                    if (_level % 2 == 0) _defensePoints += 1;
                    _agility += 1;
                    break;

                case "ARCHER":
                    _maxHitPoints += 1;
                    _maxMagicPoints += 0;
                    _attackPoints += 1;
                    if (_level % 2 == 0) _attackPoints += 1;
                    if (_level % 2 == 0) _defensePoints += 1;
                    _agility += 1;
                    break;

                case "WIZARD":
                    if (_level % 2 == 0) _maxHitPoints += 1;
                    _maxMagicPoints += 1;
                    if (_level % 2 == 0) _attackPoints += 1;
                    if (_level % 2 == 0) _defensePoints += 1;
                    _agility += 1;
                    if (_level == 4) MagicSpells[0].Level = 2;
                    break;
            }

            return returnValue;
        }

        public bool CanCastHealSpell()
        {
            return CanCastSpellWithType(Spell.Types.Heal);
        }

        public bool CanCastRecruitSpell()
        {
            return CanCastSpellWithType(Spell.Types.Recruit);
        }

        public bool CanCastAttackSpell()
        {
            return CanCastSpellWithType(Spell.Types.Attack);
        }

        public bool CanCastSpellWithType(Spell.Types type)
        {
            if (MagicSpells != null)
                for (int i = 0; i < MAXNUMBER_SPELLS; i++)
                    if (MagicSpells[i] != null && MagicSpells[i].Type == type && MagicSpells[i].MagicPoints[0] <= MagicPoints)
                        return true;

            return false;
        }

        public bool HasHealItem()
        {
            return HasItemWithType(Spell.Types.Heal);
        }

        public bool HasAttackItem()
        {
            return HasItemWithType(Spell.Types.Attack);
        }

        public bool HasItemWithType(Spell.Types type)
        {
            if (Items != null)
                for (int i = 0; i < MAXNUMBER_ITEMS; i++)
                    if (Items[i] != null && Items[i].MagicSpell != null && Items[i].MagicSpell.Type == type)
                        return true;

            return false;
        }

        public bool CanUseHealMagic()
        {
            return CanCastHealSpell() || HasHealItem();
        }

        public bool CanUseAttackMagic()
        {
            return CanCastAttackSpell() || HasAttackItem();
        }

        public void RearrangeItems(int selectedItemNumber)
        {
            bool found = false;

            for (int i = MAXNUMBER_ITEMS - 1; !found && i > selectedItemNumber; i--)
                if (Items[i] != null)
                {
                    Items[selectedItemNumber] = Items[i];
                    Items[i] = null;
                    found = true;
                }
        }

        public int ItemToLose()
        {
            if (Items != null)
                for (int i = MAXNUMBER_ITEMS - 1; i >= 0; i--)
                    if (Items[i] != null && !ItemEquipped[i])
                        return i;

            return -1;
        }

        public int FreeItemSlot()
        {
            if (Items == null)
                Items = new Item[MAXNUMBER_ITEMS];

            for (int i = 0; i < MAXNUMBER_ITEMS; i++)
                if (Items[i] == null)
                    return i;

            return -1;
        }

        public void Regenerate()
        {
            HitPoints = MaxHitPoints;
            MagicPoints = MaxMagicPoints;
        }

        public void UnBoost()
        {
            _agility -= _agilityBoostedBy;
            _agilityBoostedBy = 0;
            _defensePoints -= _defenseBoostedBy;
            _defenseBoostedBy = 0;
        }

        public void UnAttackBoost()
        {
            _attackPoints -= _attackBoostedBy;
            _attackBoostedBy = 0;
        }

    }
}