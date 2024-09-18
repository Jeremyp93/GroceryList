namespace ColruytScraper;
internal class Product
{
    public string TechnicalArticleNumber { get; set; }
    public string CommercialArticleNumber { get; set; }
    public List<string> Gtin { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string SeoBrand { get; set; }
    public string ThumbNail { get; set; }
    public string FullImage { get; set; }
    public string SquareImage { get; set; }
    public string Content { get; set; }
    public Price Price { get; set; }
    public bool IsPriceAvailable { get; set; }
    public List<string> ChannelAvailability { get; set; }
    public string BusinessDomain { get; set; }
    public int WalkRouteSequenceNumber { get; set; }
    public bool IsAvailable { get; set; }
    public bool InPromo { get; set; }
    public string TopCategoryName { get; set; }
    public string TopCategoryId { get; set; }
    public List<Category> Categories { get; set; }
    public bool IsPrivateLabel { get; set; }
    public bool IsBiffe { get; set; }
    public string WeightconversionFactor { get; set; }
    public bool IsWeightArticle { get; set; }
    public string NutriscoreLabel { get; set; }
    public bool IsBio { get; set; }
    public string CountryOfOrigin { get; set; }
    public bool IsExclusivelySoldInLuxembourg { get; set; }
    public string OrderUnit { get; set; }
    public string ShortName { get; set; }
    public bool IsNew { get; set; }
    public string EcoscoreLabel { get; set; }
    public string LongName { get; set; }
    public string RecentQuanityOfStockUnits { get; set; }
    public string AlcoholVolume { get; set; }
    public string FicCode { get; set; }
    public string EcoscoreValue { get; set; }
}

internal class Price
{
    public double BasicPrice { get; set; }
    public string RecommendedQuantity { get; set; }
    public double MeasurementUnitPrice { get; set; }
    public string MeasurementUnit { get; set; }
    public bool IsRedPrice { get; set; }
    public double PricePerUOM { get; set; }
    public string ActivationDate { get; set; }
    public string RecordSource { get; set; }
    public string IsPromoActive { get; set; }
    public string PriceChangeCode { get; set; }
}

internal class Category
{
    public string Name { get; set; }
    public string Id { get; set; }
    public List<Category> Children { get; set; }
}

internal class ProductsResponse
{
    public int ProductsFound { get; set; }
    public int ProductsReturned { get; set; }
    public int ProductsAvailable { get; set; }
    public List<Product> Products { get; set; }
}
