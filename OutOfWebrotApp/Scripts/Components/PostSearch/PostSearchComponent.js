function PostSearchComponent() {

    var self = this;

    self.tags = [];
    self.categories = [];
    self.substring = "";
    self.page = 1;

    self.beginSearch = function () {
        var data = {
            title: self.substring,
            tags: self.tags,
            categories: self.categories,
            page: self.page
        };

        $.ajax({
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            traditional: true,
            url: "/api/sitecore/Posts/GetPostsPartialView/",
            data: data,
            success: function (param) {
                $("#postSearchresultContainer").html(param);
                var r = $(".searchResultPage");
                r.on("click",
                    function (e) {
                        var page = e.toElement.innerText;
                        window.searchComponent.page = page;
                        window.searchComponent.beginSearch();
                    });
            },
            error: function (param) {
                console.log("ajax request exception: {0}", param);
            }
        });
    }
}

$(function () {

    window.searchComponent = new PostSearchComponent();

    $("#tagsTreeBox").on("changed.jstree",
        function (e, data) {
            var selectedNodes = data.instance.get_selected(true);
            var leafs = [];

            selectedNodes.forEach(function (item, i, selectedNodes) {
                if (item.children.length == 0) {
                    leafs.push(item.id);
                }
            });

            window.searchComponent.tags = leafs;
            window.searchComponent.page = 1;
            window.searchComponent.beginSearch();
        });

    $("#categoryTreeBox").on("changed.jstree",
        function (e, data) {
            var selectedNodes = data.instance.get_selected(true);
            var categories = [];

            selectedNodes.forEach(function (item, i, selectedNodes) {
                if (item.children.length == 0) {
                    categories.push(item.id);
                }
            });

            window.searchComponent.categories = categories;
            window.searchComponent.page = 1;
            window.searchComponent.beginSearch();
        });

    $("#substringInput").on("change", function () {
        var el = $("#substringInput");
        var substring = el.val();
        window.searchComponent.substring = substring;
        window.searchComponent.page = 1;
        window.searchComponent.beginSearch();
    })

    $(".searchResultPage").on("click",
        function(e) {
            var page = e.toElement.innerText;
            window.searchComponent.page = page;
            window.searchComponent.beginSearch();
        });
})