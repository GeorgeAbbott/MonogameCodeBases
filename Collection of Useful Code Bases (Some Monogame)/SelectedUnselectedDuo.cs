using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{
    /// <summary>
    /// Is a generic class that holds a duo of the same type;
    /// one used for where selected, another where not.
    /// </summary>
    class SelectedUnselectedDuo<T>
    {
        private T _selected;
        private T _unselected;

        public T SelectedValue { get; set; }
        public T UnselectedValue { get; set; }

        public T GetValue(bool isSelected)
        {
            if (isSelected) return SelectedValue;
            else return UnselectedValue;
        }

        public SelectedUnselectedDuo()
        {

        }

        public SelectedUnselectedDuo(T both) : this()
        {
            _selected = _unselected = both;
        }

        public SelectedUnselectedDuo(T selected, T unselected) : this()
        {
            _selected = selected;
            _unselected = unselected;
        }
    }

    class TODOADD { }

}
