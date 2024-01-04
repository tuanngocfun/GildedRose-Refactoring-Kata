using System;
using System.Collections.Generic;

namespace GildedRoseKata
{
    class ItemUpdater
    {
        public virtual void UpdateQuality(Item item) 
        {
            // Base implementation or make abstract if no base implementation
        }

        public virtual void UpdateSellIn(Item item) 
        {
            // Base implementation or make abstract if no base implementation
        }
    }

    class DefaultItemUpdater : ItemUpdater
    {
        private readonly Action<Item, int> _decreaseQualityAction;

        public DefaultItemUpdater(Action<Item, int> decreaseQualityAction)
        {
            _decreaseQualityAction = decreaseQualityAction;
        }

        public override void UpdateQuality(Item item) 
        {
            _decreaseQualityAction(item, 1);
            if (item.SellIn < 0)
            {
                _decreaseQualityAction(item, 1);
            }
        }
        public override void UpdateSellIn(Item item) 
        {
            item.SellIn--;
        }
    }

    class AgedBrieUpdater : DefaultItemUpdater 
    {
        public AgedBrieUpdater(Action<Item, int> decreaseQualityAction) : base(decreaseQualityAction) { }

        public override void UpdateQuality(Item item) 
        {
            GildedRose.IncreaseItemQuality(item);
            if (item.SellIn < 0)
            {
                GildedRose.IncreaseItemQuality(item);
            }
        }
    }

    class BackstagePassesUpdater : DefaultItemUpdater 
    {
        public BackstagePassesUpdater(Action<Item, int> decreaseQualityAction) : base(decreaseQualityAction) { }

        public override void UpdateQuality(Item item) 
        {
            GildedRose.IncreaseItemQuality(item);
            if (item.SellIn < 10)
            {
                GildedRose.IncreaseItemQuality(item);
            }
            if (item.SellIn < 5)
            {
                GildedRose.IncreaseItemQuality(item);
            }
            if (item.SellIn < 0)
            {
                item.Quality = 0;
            }
        }
    }

    class SulfurasUpdater : ItemUpdater 
    {
        public override void UpdateQuality(Item item) { }
        public override void UpdateSellIn(Item item) { }
    }


    public class GildedRose
    {
        public const string AGED_BRIE = "Aged Brie";
        public const string BACKSTAGE_PASSES = "Backstage passes to a TAFKAL80ETC concert";
        public const string SULFURAS = "Sulfuras, Hand of Ragnaros";

        private readonly IList<Item> _items;
        private readonly Dictionary<string, ItemUpdater> _itemUpdaters;

        public GildedRose(IList<Item> items)
        {
            _items = items;
            _itemUpdaters = new Dictionary<string, ItemUpdater>
            {
                { AGED_BRIE, new AgedBrieUpdater(DecreaseItemQuality) },
                { BACKSTAGE_PASSES, new BackstagePassesUpdater(DecreaseItemQuality) },
                { SULFURAS, new SulfurasUpdater() },
                // Default updater for items that don't match any special categories
                { "default", new DefaultItemUpdater(DecreaseItemQuality) }
            };
        }

        internal static void DecreaseItemQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Max(0, item.Quality - amount);
        }

        internal static void IncreaseItemQuality(Item item, int amount = 1, int max_quality = 50)
        {
            item.Quality = Math.Min(max_quality, item.Quality + amount);
        }

        private void UpdateQualitySingle(Item item)
        {
            var itemUpdater = _itemUpdaters.ContainsKey(item.Name) ? _itemUpdaters[item.Name] : _itemUpdaters["default"];
            
            itemUpdater.UpdateSellIn(item);
            itemUpdater.UpdateQuality(item);
        }

        public void UpdateQuality()
        {
            foreach (var item in _items)
            {
                UpdateQualitySingle(item);
            }
        }
    }
}