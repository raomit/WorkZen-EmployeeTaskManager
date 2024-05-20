let registrationForm = document.getElementById("registrationForm");

let EmployeesDataTable;
let TasksDataTable;

console.log("i am addeed")

//async function registerEmployee(employee) {
//    fetch('/Auth/Register', {
//        method: "POST",
//        body: JSON.stringify(employee)
//    }).then(response => {
//        if (!response.ok) {
//            throw new Error("network request failed");
//        } else {
//            console.log('succeeded');
//        }
//    }).catch(err => {
//        console.error(err);
//    })
//}


function getEmpManagePopUp(target) {
    debugger
    let empCode = $(target).closest('tr').find('td:first-child').text();
    fetch("/Home/GetSingleEmployeeManagePopUp", {
        body: JSON.stringify({
            empCode: empCode
        }),
        headers: {
            'Content-Type': 'application/json' // Set content type to text/plain for a simple string
        },
        method: "POST"
    }).then(reponse => {
        if (reponse.ok) {
            return reponse.text()
        }
    }).then(html => {
        console.log(html)
        debugger
        document.getElementById("manage-employee-body").innerHTML = html;
    }).catch(err => {
        console.log(err);
    })
}

function deleteEmployee(target) {
    debugger
    let empCode = $(target).closest('tr').find('td:first-child').text();
    fetch("/Home/GetSingleEmployeeDeletePopUp", {
        body: JSON.stringify({
            empCode: empCode
        }),
        headers: {
            'Content-Type': 'application/json' // Set content type to text/plain for a simple string
        },
        method: "POST"
    }).then(reponse => {
        if (reponse.ok) {
            return reponse.text()
        }
    }).then(html => {
        console.log(html)
        debugger
        document.getElementById("delete-employee-body").innerHTML = html;
    }).catch(err => {
        console.log(err);
    })
}



function getTaskPopUp(target) {
    fetch("/Tasks/GetTaskPopUp", {
        method: "GET"
    }).then(reponse => {
        if (reponse.ok) {
            return reponse.text()
        }
    }).then(html => {
        console.log(html)
        debugger
        document.getElementById("create-task-body").innerHTML = html;
    }).catch(err => {
        console.log(err);
    })
}

function editTaskPopUp(target) {
    let taskId = $(target).closest("tr").find("#taskId").text();
    debugger
    let urlToFetch = "GetModifyTaskPopUp/" + taskId;
    fetch(urlToFetch, {
        method: "GET"
    }).then(reponse => {
        if (reponse.ok) {
            return reponse.text()
        }
    }).then(html => {
        console.log(html)
        debugger
        document.getElementById("update-task-body").innerHTML = html;
    }).catch(err => {
        console.log(err);
    })
}


//function which will get list of team tasks for the manager and director
function getTeamEmployeeTasks(target) {
    debugger
    fetch("/Tasks/EmployeeTasksHome", {
        method: "GET"
    }).then(response => response.text()).then(html => {
        debugger
        document.querySelector(".content-wrapper").innerHTML = html;
        TasksDataTable = $("#DEmployeeTasks").DataTable({
            "ajax": {
                "url": "/Tasks/GetEmployeeTasksData",
                "type": "POST" // If you're sending POST requests
            },
            "serverSide": true,
            "processing": true,
            "searchable": true,
            "order": [],
            "language": {
                "emptyTable": "No Tasks Found...",
            },
            "columns": [
                {
                    data: "employeeId",
                    "autoWidth": true,
                    "searchable": true,
                },
                {
                    data: "taskDate",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    data: "taskName",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    data: "taskDescription",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    data: "createdOn",
                    "autoWidth": true,
                    "searchable": true
                },
                {
                    "data": null,
                    "autoWidth": true,
                    "sorting": false,
                    "searchable": false,
                    "render": function (data, type, row) {
                        return `
<button type="button" data-id="${row.id}" class="btn btn-gradient-success edit">Edit</button>
<button type="button" data-id="${row.id}" class="btn btn-gradient-danger delete">Delete</button>
                           `;
                    }
                }
            ],
            "columnDefs": [
                {
                    "orderable": true,
                    "targets": [0,1,2,3,4]
                },
                {
                    "orderable": false,
                    "targets": [5]
                }
            ]
        });
    }).catch(err => console.error(err));
}

//function which will get list of manager tasks for the director 
function getManagerTasks(target) {
    debugger
    fetch("/Tasks/ManagerTasksHome", {
        method: "GET"
    }).then(response => response.text()).then(html => {
        debugger
        document.querySelector(".content-wrapper").innerHTML = html;
        
    }).catch(err => console.error(err));
}

//update task status function


//TODO- please improve security here for id passing and getting from dom
function updateTaskStatus(target) {
    debugger
    let TaskId = $(target).closest("tr").find("#taskId").text();
    let TaskStatus = $(target).attr("id");
    fetch("/Tasks/ApproveOrRejectTask", {
        headers: {
            'Content-Type': 'application/json' // Set content type to text/plain for a simple string
        },
        method: "POST",
        body: JSON.stringify({
            TaskId: TaskId,
            TaskStatus: TaskStatus
        })
    }).then(response => {
        if (response.ok) {
            console.log("done");
        }
    }).catch(err => console.error(err));
}

//function in order to delete the task
function getDeleteTaskPopUp(target) {
    debugger
    let urlToFetch = "/Tasks/GetDeleteTaskPopUp/" + $(target).closest('tr').find('td[id=taskId]').text();
    fetch(urlToFetch, {
        method: "GET"
    }).then(reponse => {
        if (reponse.ok) {
            return reponse.text()
        }
    }).then(html => {
        console.log(html)
        debugger
        document.getElementById("delete-task-body").innerHTML = html;
    }).catch(err => {
        console.log(err);
    })
}

//function deleteTask(target) {
//    debugger
//    let taskId = $(target).closest('form').find('input').val();
//    fetch("Tasks/DeleteTask", {
//        method: "POST",
//        headers: {
//            'Content-Type': 'application/json' // Set content type to text/plain for a simple string
//        },
//        body: JSON.stringify({
//            TaskId: taskId
//        })
//    }).then(reponse => {
//        if (reponse.ok) {
//            return reponse.text()
//        }
//    }).then(html => {
//        console.log(html)
//        debugger
//        document.getElementById("delete-task-body").innerHTML = html;
//    }).catch(err => {
//        console.log(err);
//    })
//}


//registrationForm.addEventListener("submit", function (event) {
//    debugger

//    if (!registrationForm.checkValidity()) {
//        event.preventDefault();
//        event.stopPropagation();
//        registrationForm.classList.add("was-validated");
//    } else {
//        let employee = {};
//        Array.from(document.getElementsByClassName('form-input-group')).forEach(function (ele) {
//            debugger
//            employee[$(ele).find("input")[0].id] = $(ele).find("input").val();
//        });
//        employee[$('.form-select-group').find('select')[0].id] = $('.form-select-group').find('select')[0].value;
//    }
//})