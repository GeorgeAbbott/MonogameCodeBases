using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{
    partial class Menu
    {
        public int ItemWidth { get; }
        public int ItemHeight { get; }

        // Below for adding another class which is called and rendered
        public bool DrawNonHighestLevelMenu = true;
        private Menu Submenu = null;


        // Note: Selection use a zero index
        private int FirstSelection { get { return 0; } }
        private int FinalSelection { get { return ItemHeight - 1; } }
        private int CurrentSelection
        {
            get { return CurrentSelection; }
            set
            {
                if (CurrentSelection < FirstSelection) CurrentSelection = FinalSelection;
                if (CurrentSelection > FinalSelection) CurrentSelection = FirstSelection;
            }
        }
        private bool IsSelectionMade { get; set; }

        private Vector2 Coordinates { get; } // Top left coordinates
        private Texture2D Background { get; set; }
        public bool CalculateOffsets { get; set; } // Whether to render with offsets or positions for each menu component
        public Vector2 Offsets { get; set; } // What the offsets are if using them

        public void Draw(SpriteBatch sb)
        {
            if (DrawNonHighestLevelMenu)
            {
                // Render the Menu Background
                sb.Draw(Background, Coordinates);


                // Pass onto the pages to render the subsequent parts
                foreach (Page page in Pages)
                {
                    page.Draw(sb);
                }
            }

            if (Submenu != null)
                Submenu.Draw(sb);

        }

        public void HandleInput(InputHandler ih)
        {
            if (ih.IsKeyDown(Keys.Up, 0.3)) // URGENTTODO: replace these magic numbers (timegaps) with Constants
            {
                CurrentSelection--;
            }
            if (ih.IsKeyDown(Keys.Down, 0.3))
                CurrentSelection++;
            if (ih.IsKeyDown(Keys.Enter))
            {
                // TODO: make selection, if this means going to the new menu do so
            }
        }



        /// <param name="width">The entries needed widthwise for all Entries.</param>
        /// <param name="height">The maximum entries per page.</param>
        /// <param name="topleftCoords">The top left coordinates of the Menu, i.e. the render position.</param>
        public Menu(int width, int height, Vector2 topleftCoords)
        {
            ItemWidth = width;
            ItemHeight = height;

            Coordinates = topleftCoords;

            CalculateOffsets = false;

        }

        public Menu(int width, int height, Vector2 topLeftCoords, Vector2 offsets)
            : this(width, height, topLeftCoords)
        {
            Offsets = offsets;
            CalculateOffsets = true;
        }
    }

    partial class Menu
    {
        // This part of the code deals with everything to do with pages, 
        // including the definition of the Page class within Menu.

        private List<Page> Pages { get; set; }
        private int CurrentPagePos { get; set; } // Works on a zero index.
        private Page CurrentPage { get { return Pages[CurrentPagePos]; } }

        public void AddNewEmptyPage (bool selectNewPage = true)
        {
            Pages.Add(new Page(this));
            if (selectNewPage)
                CurrentSelection = FinalSelection;
        }

        public void AddNewEntry(MenuEntry entry)
        {
            if (CurrentPage.IsFull) AddNewEmptyPage(selectNewPage: true);

            CurrentPage.AddMenuEntry(entry);

        }

        class Page
        {
            // Page should be defined within Menu both because it is privy to
            // the Width and Height of the Menu, but also because it is instantianted
            // only within the Menu class.

            Menu menu_; // Used to access variables from Menu

            private List<MenuEntry> Entries { get; set; }

            public int PageWidth { get { return menu_.ItemWidth; } }

            public bool IsFull
            {
                get { return Entries.Count >= menu_.ItemWidth; }
            }

            public void AddMenuEntry(MenuEntry menuEntry)
            {
                if (menuEntry.Width != PageWidth)
                    throw new ArgumentException($"menuEntry does not have the correct width of {menu_.ItemWidth}");

                if (!IsFull) Entries.Add(menuEntry);

            }

            public void Draw(SpriteBatch sb)
            {
                // If menu_.RenderOffsets is true, then calculate offsets for Y, else just Draw.

                if (menu_.CalculateOffsets)
                {
                    // Calculate the Y offsets for each MenuEntry. X is calculated in MenuEntry.Draw
                    for (int i = 0; i < Entries.Count; i++)
                    {
                        Entries[i].Draw(sb, menu_.Coordinates,
                            new Vector2(menu_.Offsets.X, (menu_.Offsets.Y * (i + 1))));
                    }
                }
                else
                {
                    // Allow the menu items to calculate offsets, and hence to not pass any position arguments.
                    foreach (MenuEntry menuEntry in Entries)
                    {
                        menuEntry.Draw(sb);
                    }
                }




            }


            public Page (Menu menu)
            {
                menu_ = menu;
            }



        }
    }

    class MenuEntry
    {
        public int Width;

        private List<MenuGenericItem> components;

        public MenuEntry(Menu o_)
        {
            Width = o_.ItemWidth;
        }

        public void Draw(SpriteBatch sb)
        { // Should be called in Page where Menu.CalculateOffsets == false.

            foreach (MenuGenericItem component in components)
            {
                component.Draw(sb);
            }
        }

        public void Draw(SpriteBatch sb, Vector2 topleft, Vector2 offsets)
        {
            // The Y offset should already be calculated by Page. 
            // Hence, this calculates the x offsets only.
            // Then adds it to the topleft, resulting in the actual
            // position that is then passed to the menu components.

            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(sb, Vector2.Add(topleft, new Vector2(offsets.X * (i+1), offsets.Y)));
            }

        }
        



    }

    interface IMenuComponent
    {
        bool DefinesOwnCoordinates { get; }

        void Draw(SpriteBatch sb);
    }

    abstract class MenuGenericItem : IMenuComponent
    {
        public bool DefinesOwnCoordinates
        {
            get
            {
                return !(Position == null);
            }
        }

        protected Position Position;

        /// <summary>
        /// Call this overload where the object defines its own coordinates.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        { // NOTE: do not override or add the override modifier; all drawing is done in Draw(SpriteBatch, Position)
            if (!DefinesOwnCoordinates)
                throw new ArgumentException("Does not define own coordinates; use Draw(SpriteBatch, Position) or define coordinates");

            Draw(sb, Position);
        }
        /// <summary>
        /// Call this overload where the object does not define its own coordinates, or you wish to specify the position to render.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="position"></param>
        public abstract void Draw(SpriteBatch sb, Position position); // needs definition in derived classes




    }


    class MenuText : MenuGenericItem, IMenuComponent
    { // TODO: add

        SelectedUnselectedDuo<Color> colors;
        string text;
        SpriteFont font;

        public override void Draw(SpriteBatch sb, Position position)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">If kept as null, it is assumed that position is calculated as an offset.</param>
        public MenuText (Position position = null)
        {
            Position = position;
        }

    }

    class MenuImage : MenuGenericItem, IMenuComponent
    {
        // TODO: add rest


        public override void Draw(SpriteBatch sb, Position position)
        {
            throw new NotImplementedException();
        }
    }

}

/*
 * Each menu should be able to:
 *  YES - hold x amount of items (ItemWidth) 
 *  YES - have y amount of pages
 *  YES - have a background
 *  YES - have a coordinate
 *  YES - have a current selection, and likewise current page
 *  YES - have position of each item either
 *      - calculated with a regular offset from the previous entry, or from the topleft
 *      - have the position overtly specified for the entry
 *  PARTIAL be either an Image or Text
 *  - have the following properties for each entry, which can be used, e.g. for color
 *      - IsSelected: whether or not this is currently selected
 *      - IsMethod:   checks whether a method returns true, i.e. player has enough money,
 *                    if not marks the entry red.
 *      *  - have multiple LEVELs of menu, e.g. when a selection made for one item, it goes to the next level
 *      - this should be possible with any number of levels
 *          - probably best to implement as an instance of the Menu class itself, which this menu
 *            class passes onto for the input handling, and when draw calls, checks DrawNonHighestLevelMenu
 *            and if false does not draw current menu, only new menu.
 * 
 * 
 */
 /*
  * Be Either Image or Text has yet to be fully made, MenuImage and MenuText classes still need info.
  * 
  * */