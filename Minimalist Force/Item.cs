using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame2
{
    internal class Item
    {
        public static Texture2D TextureItemSelected;

        public static Texture2D TextureAngelWing;
        public static Texture2D TextureAntidote;
        public static Texture2D TextureMedicalHerb;

        public static Texture2D TextureNothing;

        public Texture2D Texture;

        private string _name1;

        public string Name1
        {
            get { return _name1; }
            set { _name1 = value; }
        }

        private string _name2;

        public string Name2
        {
            get { return _name2; }
            set { _name2 = value; }
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

        private int _agility;

        public int Agility
        {
            get { return _agility; }
            set { _agility = value; }
        }

        private int _movePoints;

        public int MovePoints
        {
            get { return _movePoints; }
            set { _movePoints = value; }
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

        public static int CurrentState;

        public Spell MagicSpell;

        private bool _canGetBroken;

        public bool CanGetBroken
        {
            get { return _canGetBroken; }
            set { _canGetBroken = value; }
        }

        private int _usesBeforeBroken;

        public int UsesBeforeBroken
        {
            get { return _usesBeforeBroken; }
            set { _usesBeforeBroken = value; }
        }

        private bool _canBeUsedOnlyOnce;

        public bool CanBeUsedOnlyOnce
        {
            get { return _canBeUsedOnlyOnce; }
            set { _canBeUsedOnlyOnce = value; }
        }

        public Item(string name1, string name2)
        {
            _name1 = name1;
            _name2 = name2;
            _canGetBroken = false;
            _canBeUsedOnlyOnce = false;
            switch (_name1 + _name2)
            {
                // *** all items have to be added to this list

                case "Angel Wing":
                    Texture = TextureAngelWing;
                    _canBeUsedOnlyOnce = true;
                    MagicSpell = new Spell("EGRESS", 1);
                    break;

                case "Antidote":
                    Texture = TextureAntidote;
                    _canBeUsedOnlyOnce = true;
                    MagicSpell = new Spell("DETOX", 1);
                    break;

                case "Medical Herb":
                    Texture = TextureMedicalHerb;
                    _canBeUsedOnlyOnce = true;
                    MagicSpell = new Spell("HEAL", 1);
                    break;

                // weapons

                case "Nothing":
                    Texture = TextureNothing;
                    MagicSpell = null;
                    _minAttackRange = 0;
                    _maxAttackRange = 0;
                    break;
            }
            CurrentState = 0;
        }

        public void DrawAt(SpriteBatch spriteBatch, Vector2 position)
        {
            DrawAt(spriteBatch, position, false);
        }

        public void DrawAt(SpriteBatch spriteBatch, Vector2 position, bool selected)
        {
            spriteBatch.Draw(Texture, position, Color.White);

            if (selected && CurrentState == 1)
                spriteBatch.Draw(TextureItemSelected, position, Color.White);
        }

        public bool CanBeEquipped(string charClass)
        {
            // *** is to be extended to cover all character classes and weapon types
            switch (charClass)
            {
                case "ACHR":
                    switch (_name2)
                    {
                        case " Arrow":
                        case " Shell":
                            return true;

                        default:
                            return false;
                    }

                case "BDMN":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "BDBT":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "BRN":
                    switch (_name2)
                    {
                        case " Axe":
                            return true;

                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "BWNT":
                    switch (_name2)
                    {
                        case " Arrow":
                            return true;

                        case " Shell":
                            return true;

                        default:
                            return false;
                    }

                case "GLDT":
                    switch (_name2)
                    {
                        case " Axe":
                            return true;

                        default:
                            return false;
                    }

                case "HERO":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "KNTE":
                    switch (_name2)
                    {
                        case " Lance":
                            return true;

                        case " Spear":
                            return true;

                        default:
                            return false;
                    }

                case "MAGE":
                    switch (_name2)
                    {
                        case " Staff":
                            return true;

                        case " Stick":
                            return true;

                        default:
                            switch (_name1)
                            {
                                case "Flail":
                                    return true;
                            }
                            return false;
                    }

                case "NINJ":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "PLDN":
                    switch (_name2)
                    {
                        case " Lance":
                            return true;

                        case " Spear":
                            return true;

                        default:
                            return false;
                    }

                case "PRST":
                    switch (_name2)
                    {
                        case " Staff":
                            return true;

                        case " Stick":
                            return true;

                        default:
                            switch (_name1)
                            {
                                case "Flail":
                                    return true;
                            }
                            return false;
                    }

                case "RNGR":
                    switch (_name2)
                    {
                        case " Arrow":
                            return true;

                        case " Shell":
                            return true;

                        default:
                            return false;
                    }

                case "SNIP":
                    switch (_name2)
                    {
                        case " Arrow":
                            return true;

                        case " Shell":
                            return true;

                        default:
                            return false;
                    }

                case "SDMN":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        default:
                            return false;
                    }

                case "THIF":
                    switch (_name2)
                    {
                        case " Dagger":
                            return true;

                        case " Knife":
                            return true;

                        default:
                            return false;
                    }

                case "VICR":
                    switch (_name2)
                    {
                        case " Staff":
                            return true;

                        case " Stick":
                            return true;

                        default:
                            switch (_name1)
                            {
                                case "Flail":
                                    return true;
                            }
                            return false;
                    }

                case "WARR":
                    switch (_name2)
                    {
                        case " Sword":
                            return true;

                        case " Axe":
                            return true;

                        default:
                            return false;
                    }

                case "WIZ":
                    switch (_name2)
                    {
                        case " Staff":
                            return true;

                        case " Stick":
                            return true;

                        default:
                            switch (_name1)
                            {
                                case "Flail":
                                    return true;
                            }
                            return false;
                    }

                default:
                    return false;
            }
        }
    }
}
