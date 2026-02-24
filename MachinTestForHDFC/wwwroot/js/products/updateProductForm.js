$(document).ready(function () {

    var selectedCategoryId = $("#CategoryId").data('selected');
    loadCategoryDropdown(selectedCategoryId);
})

function loadCategoryDropdown(selectedCategoryId) {
    $.ajax({
        url: '/Category/GetAllCategories',
        type: 'GET',
        success: function (data) {
            var dropdown = $("#CategoryId");
            dropdown.empty();

            dropdown.append('<option value="">--Select Category--</option>');

            $.each(data, function (index, cat) {
                var isSelected = cat.id == selectedCategoryId ? 'selected' : '';
                dropdown.append('<option value="' + cat.id + '" ' + isSelected + '> ' + cat.categoryName + '</option>');
            });
        },
        error: function (err) {
            console.log("Error loading departments: " + err);
        }
    })
}