// Название контроллера, для работы с подкаталогами
let SubCatalogsControllerName = "SubCatalog";
// Название метода для получения каталогов
let SubCatalogsMethodName = "SubCatalogCatalogs";
//Название контроллера, для работы со статьями
let SubCatalogArticlesControllerName = "Article";
// Название метода для получения статей
let SubCatalogArticlesMethodName = "CatalogArticles";

function getSubCatalogs(elementId, parentCatalogId) {
    $('#' + elementId).load(
        "/" + SubCatalogsControllerName +
        "/" + SubCatalogsMethodName +
        "/?parentId=" + parentCatalogId);
}

function getSubCatalogArticles(elementId, catalogId) {
    $('#' + elementId).load(
        "/" + SubCatalogArticlesControllerName +
        "/" + SubCatalogArticlesMethodName +
        "/?catalogId=" + catalogId);
}

//  Вспомогательная ф-ия, которая срабатывает при нажатии на элементах 
//  для отображения подкаталогов и статей. Изначально блокам ставится values == false => блоки закрыты
function LoadHideShowElements(element, CategoryOrArticles) {
    var classes = element.classList;
    var elementId = classes[1];
    var catalogId = classes[2];
    if (CategoryOrArticles === true)
        getSubCatalogs(elementId, catalogId);
    else
        getSubCatalogArticles(elementId, catalogId);

    if ($(element).attr("value") == "false") {
        $("#" + elementId).show();
        $(element).attr("value", "true");
    }
    else {
        $("#" + elementId).hide();
        $(element).attr("value", "false");
    }
}

// Установить событие (клик) на элементы с классом subCatalogs
$(document).on("click", ".subCatalogs", function () {
    LoadHideShowElements(this, true);
});

$(document).on("click", ".subCatalogArticles", function () {
    LoadHideShowElements(this, false);
});

// Отправить ajax запрос на удаление подкаталока по его идентификатору
$(document).on("click", ".deleteSubCatalog", function () {
    var subCatalogId = $(this).attr("id");
    $.get("/SubCatalog/DeleteSubCatalog", { catalogId : subCatalogId});
    $("#" + subCatalogId).remove();
});