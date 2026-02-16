$(document).ready(function () {

    let isDuplicate = false;
    //Generate employee code.
    $.get('/Employees/GenerateEmpCode', function (data) {
        $("#EmpCode").val(data);
    });

    loadDropdown();

    $("#EmpCode").on("blur", function () {
        var empCode = $(this).val();
        if (empCode === "")
            return;

        $.ajax({
            url: '/Employees/CheckDuplicateEmpCode',
            type: 'GET',
            data: { empCode: empCode },
            success: function (isExists) {
                if (isExists) {
                    isDuplicate = isExists;
                    $("span[data-valmsg-for='EmpCode']")
                        .text("Employee Code already exists.")
                        .show();
                }
                else {
                    $("span[data-valmsg-for='EmpCode']")
                        .text("")
                        .hide();
                }
            }
        });
    });

    $("form").submit(function (e) {
        if (isDuplicate) {
            e.preventDefault();

            $("span[data-valmsg-for='EmpCode']")
                .text("Employee Code already exists.")
                .show();

            $("#EmpCode").focus();
        }
    });

    //Validate form before submission.
    //$("form").submit(function (e) {
    //    var isValid = true;

    //    var fullName = $("#FullName").val().trim();
    //    var empCode = $("#EmpCode").val().trim();
    //    var departmentId = $("#DepartmentId").val().trim();
    //    var salary = $("#Salary").val().trim();
    //    var dob = $("#DOB").val().trim();

    //    $(".error").remove();

    //    if (fullName === "") {
    //        showError("#FullName", "Please enter value in Full Name.");
    //        isValid = false;
    //    }

    //    if (empCode === "") {
    //        showError("#EmpCode", "Please enter value in Employee Code.");
    //        isValid = false;
    //    }

    //    if (departmentId === "") {
    //        showError("#DepartmentId", "Please select department.");
    //        isValid = false;
    //    }

    //    if (salary === "" || salary <= "0") {
    //        showError("#Salary", "Please enter valid Salary and Salary should be greater than zero.");
    //        isValid = false;
    //    }

    //    if (dob === "") {
    //        showError("#DOB", "Please select Date of Birth.");
    //        isValid = false;
    //    }

    //    if (!isValid) {
    //        e.preventDefault();
    //    }
    //});

    //Calculate tax on salary.

    $("#btnCalculateTax").click(function () {
        var salary = $("#Salary").val();

        if (salary <= 0) {
            alert("Salary should be greater than zero.");
            return;
        }
        $.ajax({
            url: '/Employees/CalculateTax',
            type: 'POST',
            data: { salary: salary },
            success: function (data) {
                $("#taxResult").text(data);
            },
            error: function () {
                alert("Error occur while calculating tax.");
            }
        });
    });
});

function loadDropdown() {
    $.ajax({
        url: '/Department/GetAllDepartments',
        type: 'GET',
        success: function (data) {
            var dropDown = $("#DepartmentId");
            dropDown.empty();

            dropDown.append('<option value="">--Select Department--</option>');

            $.each(data, function (index, dept) {
                dropDown.append('<option value="' + dept.id + '">' + dept.name + '</option>');
            });
        },
        error: function (err) {
            console.log("Error loading departments: " + err);
        }
    })
}

//function checkDuplicateEmpCode(empCode, id) {
//    fetch(`/api/employees/checkDuplicateEmpCodeAsync?empCode=${empCode}&id=${id ?? ''}`)
//        .then(response => response.json())
//        .then(data => {
//            if (data) {
//                document.getElementById("empCodeMsg").textContent = "Employee code already exists.";
//            }
//            else {
//                document.getElementById("empCodeMsg").textContent = "";
//            }
//        })
//}

//function showError(element, message) {
//    var input = $(element);

//    input.next(".error").remove();

//    $(element).after('<span class="text-danger error">' + message + '</span>');
//}