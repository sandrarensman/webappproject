using HtmlAgilityPack;
using SchoolApp.Helpers;

namespace SchoolApp.Test.UnitTest.Helpers;

public class DropdownHtmlHelperTest
{
    [Fact]
    public void DropdownHtmlHelper_ShouldReturnValidHtml()
    {
        // Arrange
        var pageSize = 10;
        var doc = new HtmlDocument();

        // Act
        var dropdownHtml = DropdownHtmlHelper.GenerateDropdownHtml(pageSize);
        doc.LoadHtml(dropdownHtml);

        // Assert
        Assert.NotNull(doc.DocumentNode);
        Assert.NotEmpty(doc.DocumentNode.InnerHtml);
    }

    [Fact]
    public void GenerateDropdownHtml_ShouldContainExpectedElements()
    {
        // Arrange
        var pageSize = 10;
        var doc = new HtmlDocument();

        // Act
        var dropdownHtml = DropdownHtmlHelper.GenerateDropdownHtml(pageSize);
        doc.LoadHtml(dropdownHtml);

        // Assert
        var formNode = doc.DocumentNode.SelectSingleNode("//form");
        var selectNode = doc.DocumentNode.SelectSingleNode("//select[@name='pageSize']");
        var optionNodes = doc.DocumentNode.SelectNodes("//option");

        Assert.NotNull(formNode);
        Assert.NotNull(selectNode);
        Assert.NotNull(optionNodes);
        Assert.Equal(3, optionNodes.Count);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void GenerateDropdownHtml_ShouldMarkCorrectOptionAsSelected(int pageSize)
    {
        // Arrange
        var doc = new HtmlDocument();

        // Act
        var dropdownHtml = DropdownHtmlHelper.GenerateDropdownHtml(pageSize);
        doc.LoadHtml(dropdownHtml);

        // Assert
        var selectedOption = doc.DocumentNode.SelectSingleNode("//option[@selected]");
        Assert.NotNull(selectedOption);
        Assert.Equal(pageSize.ToString(), selectedOption.GetAttributeValue("value", ""));
    }

    [Fact]
    public void GenerateDropdownHtml_ShouldHaveCorrectFormAndSelectAttributes()
    {
        // Arrange
        var pageSize = 10;
        var doc = new HtmlDocument();

        // Act
        var dropdownHtml = DropdownHtmlHelper.GenerateDropdownHtml(pageSize);
        doc.LoadHtml(dropdownHtml);

        // Assert
        var formNode = doc.DocumentNode.SelectSingleNode("//form");
        var selectNode = doc.DocumentNode.SelectSingleNode("//select");

        Assert.NotNull(formNode);
        Assert.Equal("get", formNode.GetAttributeValue("method", ""));
        Assert.Equal("display: inline; margin: 0; padding: 0; border: none;", formNode.GetAttributeValue("style", ""));

        Assert.NotNull(selectNode);
        Assert.Equal("pageSize", selectNode.GetAttributeValue("name", ""));
        Assert.Equal("this.form.submit()", selectNode.GetAttributeValue("onchange", ""));
        Assert.Equal("page-size-dropdown", selectNode.GetAttributeValue("class", ""));
    }

    [Fact]
    public void GenerateDropdownHtml_ShouldThrowArgumentException_WhenPageSizeIsNegative()
    {
        // Arrange
        var pageSize = -10;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DropdownHtmlHelper.GenerateDropdownHtml(pageSize));
    }
}