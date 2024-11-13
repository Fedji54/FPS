using UnityEngine;

namespace WinterUniverse
{
    public class ItemConfig : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] protected string _displayName = "Name";
        [SerializeField, TextArea] protected string _description = "Description";
        [SerializeField] protected Sprite _icon;
        [SerializeField] protected ItemType _itemType;
        [SerializeField] protected GameObject _model;
        [SerializeField] protected float _weight = 1f;
        [SerializeField] protected int _maxCountInStack = 1;
        [SerializeField] protected int _price = 100;
        [SerializeField] protected float _rating = 1f;

        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;
        public GameObject Model => _model;
        public float Weight => _weight;
        public int MaxCountInStack => _maxCountInStack;
        public int Price => _price;
        public float Rating => _rating;

        public virtual void Use(PawnController character, bool fromInventory = true)
        {

        }
    }
}