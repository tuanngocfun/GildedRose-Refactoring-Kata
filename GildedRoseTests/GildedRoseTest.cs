using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void Foo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual("foo", items[0].Name);
    }

    [Test]
    public void QualityDegradesByOneEverydayForNormalItems() 
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 1, Quality = 1 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(0, items[0].Quality);
    }

    [Test]
    public void QualityDegradesTwiceAsFastAfterSellInDate() 
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 2 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(0, items[0].Quality);
    }

    [Test]
    public void QualityIsNeverNegative() 
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(0, items[0].Quality);
    }

    [Test]
    public void AgedBrieIncreasesInQuality() 
    {
        var items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(1, items[0].Quality);
    }

    // Loop from day 1 to the end of the month (30 days) so that it can be tested that the quality degrades twice as fast when passing expiration date
    [Test]
    public void QualityIsNeverMoreThan50() 
    {
        var items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 1, Quality = 0 } };
        var app = new GildedRose(items);
        for (int i = 0; i < 30; i++)
        {
            app.UpdateQuality();
        }
        Assert.AreEqual(50, items[0].Quality);
    }

    [Test]
    public void SulfurasNeverHasToBeSoldOrDecreasesInQuality() 
    {
        var items = new List<Item> { new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 1, Quality = 80 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(80, items[0].Quality);
        Assert.AreEqual(1, items[0].SellIn);
    }

    [Test]
    public void backstagePassesIncreaseInQualityBy2WhenSellInIs10OrLess() 
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 10 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(12, items[0].Quality);
    }

    [Test]
    public void backstagePassesIncreaseInQualityBy3WhenSellInIs5OrLess() 
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 10 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(13, items[0].Quality);
    }

    [Test]
    public void backstagePassesQualityDropsTo0AfterConcert() 
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 10 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.AreEqual(0, items[0].Quality);
    }
}