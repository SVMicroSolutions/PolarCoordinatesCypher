using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher.Tests;

[TestClass]
public class StarCatalogTests
{
    [TestMethod]
    public void Entries_ContainsAllTwentySixLetters()
    {
        Assert.HasCount(26, StarCatalog.Entries);
    }

    [TestMethod]
    [DataRow('A', "Acrux", 14, 22, "01422")]
    [DataRow('S', "Sirius", 266, 99, "26699")]
    [DataRow('T', "Thuban", 280, 36, "28036")]
    [DataRow('R', "Rigel", 252, 17, "25217")]
    [DataRow('V', "Vega", 308, 28, "30828")]
    [DataRow('E', "Elnath", 70, 35, "07035")]
    [DataRow('G', "Gacrux", 98, 27, "09827")]
    [DataRow('Z', "Zubenelgenubi", 359, 44, "35944")]
    public void GetByLetter_ReturnsExpectedStarKeyData(char letter, string starName, int theta, int radius, string expectedCode)
    {
        StarKeyEntry entry = StarCatalog.GetByLetter(letter);

        Assert.AreEqual(starName, entry.StarName);
        Assert.AreEqual(theta, entry.ThetaDegrees);
        Assert.AreEqual(radius, entry.Radius);
        Assert.AreEqual(expectedCode, entry.Code);
    }

    [TestMethod]
    public void GetByLetter_IsCaseInsensitive()
    {
        StarKeyEntry upper = StarCatalog.GetByLetter('A');
        StarKeyEntry lower = StarCatalog.GetByLetter('a');

        Assert.AreEqual(upper, lower);
    }

    [TestMethod]
    public void GetByLetter_ThrowsForNonLetter()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => StarCatalog.GetByLetter('5'));
    }

    [TestMethod]
    public void ConstellationCode_IsPrefixedWithLetter()
    {
        StarKeyEntry entry = StarCatalog.GetByLetter('S');

        Assert.AreEqual("S26699", entry.ConstellationCode);
    }

    [TestMethod]
    public void TryGetByCode_FindsEntryForKnownCode()
    {
        bool found = StarCatalog.TryGetByCode("26699", out StarKeyEntry? entry);

        Assert.IsTrue(found);
        Assert.IsNotNull(entry);
        Assert.AreEqual('S', entry!.Letter);
    }

    [TestMethod]
    public void TryGetByCode_ReturnsFalseForUnknownCode()
    {
        bool found = StarCatalog.TryGetByCode("99999", out StarKeyEntry? entry);

        Assert.IsFalse(found);
        Assert.IsNull(entry);
    }
}
