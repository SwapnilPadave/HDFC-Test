$(document).ready(function () {

    loadCategoryDropdown();

    $("form").submit(function (e) {
        var isValid = true;
        var categoryId = $("#CategoryId").val();
        $(".error").remove();

        if (!categoryId || parseInt(categoryId) <= 0) {
            showError("#CategoryId", "Please select category.");
            isValid = false;
        }
        if (!isValid) {
            e.preventDefault();
        }
    })
});

function loadCategoryDropdown() {
    $.ajax({
        url: '/Category/GetAllCategories',
        type: 'GET',
        success: function (data) {
            var dropdown = $("#CategoryId");
            dropdown.empty();

            dropdown.append('<option value="">--Select Category--</option>');

            $.each(data, function (index, cat) {
                dropdown.append('<option value="' + cat.id + '">' + cat.categoryName + '</option>');
            });
        },
        error: function (err) {
            console.log("Error loading departments: " + err);
        }
    })
}

function showError(element, message) {
    var input = $(element);

    input.next(".error").remove();

    $(element).after('<span class="text-danger error">' + message + '</span>');
}