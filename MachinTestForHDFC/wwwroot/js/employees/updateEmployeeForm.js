$(document).ready(function () {

    var selectedDepartmentId = $("#DepartmentId").data('selected')

    loadDropdown(selectedDepartmentId);

    $("#btnCalculateTax").click(function () {
        var salary = $("#Salary").val();

        if (salary <= 0) {
            alert("Enter valid salary.");
            return;
        }

        $.ajax({
            url: "/Employees/CalculateTax",
            type: "POST",
            data: { salary: salary },
            success: function (data) {
                $("#taxResult").text(data);
            },
            error: function () {
                alert("Error calculating tax.");
            }
        });
    });
});

function loadDropdown(selectedId) {
    $.ajax({
        url: '/Department/GetAllDepartments',
        type: 'GET',
        success: function (data) {
            var dropDown = $("#DepartmentId");
            dropDown.empty();

            dropDown.append('<option value="">--Select Department--</option>');

            $.each(data, function (index, dept) {

                var isSelected = dept.id == selectedId ? 'selected' : '';

                dropDown.append('<option value="' + dept.id + '" ' + isSelected + '> ' + dept.name + '</option > ');
            });
        },
        error: function (err) {
            console.log("Error loading departments: " + err);
        }
    })
}