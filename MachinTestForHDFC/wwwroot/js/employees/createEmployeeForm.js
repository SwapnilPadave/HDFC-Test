$(document).ready(function () {

    let isDuplicate = false;

    //Generate employee code.
    $.get('/Employees/GenerateEmpCode', function (data) {
        $("#EmpCode").val(data);
    });

    //Load dropdowns
    loadDepartmentDropdown();
    loadCountryDropdown();

    //Load states by country change
    $("#CountryId").change(function () {
        var countryId = $(this).val();

        $("#StateId").empty().append('<option value="">--Select State--</option>');
        $("#CityId").empty().append('<option value="">--Select City--</option>');

        if (countryId) {
            loadStateDropdown(countryId);
        }
    });

    //Load cities by state change
    $("#StateId").change(function () {
        var stateId = $(this).val();

        $("#CityId").empty().append('<option value="">--Select City--</option>');

        if (stateId) {
            loadCityDropdown(stateId);
        }
    });

    //Validation to check duplicate empcode
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

    //form submit validation function
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

    $("#btnCalculateTax").click(function () {
    //Calculate tax on salary.
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

//function to load department dropdown
function loadDepartmentDropdown() {
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

//function to load country dropdown
function loadCountryDropdown() {
    $.ajax({
        url: '/CountryStateCity/GetAllCountries',
        type: 'GET',
        success: function (data) {
            var dropdown = $("#CountryId");
            dropdown.empty();

            dropdown.append('<option value="">--Select Country--</option>');

            $.each(data, function (index, country) {
                dropdown.append('<option value="' + country.id + '">' + country.countryName + '</option>');
            });
        },
        error: function (err) {
            console.log("Error loading countries: " + err);
        }
    })
}

//function to load state dropdown
function loadStateDropdown(countryId) {
    $.get('/CountryStateCity/GetAllStateByCountryId', { countryId: countryId }, function (data) {
        $.each(data, function (i, item) {
            $("#StateId").append(
                $('<option>', {
                    value: item.id,
                    text: item.stateName
                })
            );
        });
    });
}

//function to load city dropdown
function loadCityDropdown(stateId) {
    $.get('/CountryStateCity/GetAllCityByStateId', { stateId: stateId }, function (data) {
        $.each(data, function (i, item) {
            $("#CityId").append(
                $('<option>', {
                    value: item.id,
                    text: item.cityName
                })
            );
        });
    });
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