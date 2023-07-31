// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// #region Toggle Password eyes

function togglePasswordInputType() {
    var x = document.getElementById("Input_Password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function toggleConfirmPasswordInputType() {
    var x = document.getElementById("Input_ConfirmPassword");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

// #endregion

// #region Pipelining function for DataTables

//
// Pipelining function for DataTables. To be used to the `ajax` option of DataTables
//
$.fn.dataTable.pipeline = function (opts) {
    // Configuration options
    var conf = $.extend(
        {
            pages: 5, // number of pages to cache
            url: '', // script url
            data: null, // function or object with parameters to send to the server
            // matching how `ajax.data` works in DataTables
            method: 'GET', // Ajax HTTP method
        },
        opts
    );

    // Private variables for storing the cache
    var cacheLower = -1;
    var cacheUpper = null;
    var cacheLastRequest = null;
    var cacheLastJson = null;

    return function (request, drawCallback, settings) {
        var ajax = false;
        var requestStart = request.start;
        var drawStart = request.start;
        var requestLength = request.length;
        var requestEnd = requestStart + requestLength;

        if (settings.clearCache) {
            // API requested that the cache be cleared
            ajax = true;
            settings.clearCache = false;
        } else if (cacheLower < 0 || requestStart < cacheLower || requestEnd > cacheUpper) {
            // outside cached data - need to make a request
            ajax = true;
        } else if (
            JSON.stringify(request.order) !== JSON.stringify(cacheLastRequest.order) ||
            JSON.stringify(request.columns) !== JSON.stringify(cacheLastRequest.columns) ||
            JSON.stringify(request.search) !== JSON.stringify(cacheLastRequest.search)
        ) {
            // properties changed (ordering, columns, searching)
            ajax = true;
        }

        // Store the request for checking next time around
        cacheLastRequest = $.extend(true, {}, request);

        if (ajax) {
            // Need data from the server
            if (requestStart < cacheLower) {
                requestStart = requestStart - requestLength * (conf.pages - 1);

                if (requestStart < 0) {
                    requestStart = 0;
                }
            }

            cacheLower = requestStart;
            cacheUpper = requestStart + requestLength * conf.pages;

            request.start = requestStart;
            request.length = requestLength * conf.pages;

            // Provide the same `data` options as DataTables.
            if (typeof conf.data === 'function') {
                // As a function it is executed with the data object as an arg
                // for manipulation. If an object is returned, it is used as the
                // data object to submit
                var d = conf.data(request);
                if (d) {
                    $.extend(request, d);
                }
            } else if ($.isPlainObject(conf.data)) {
                // As an object, the data given extends the default
                $.extend(request, conf.data);
            }

            return $.ajax({
                type: conf.method,
                url: conf.url,
                data: JSON.stringify(request), // request,
                dataType: 'json',
                contentType: "application/json",
                cache: false,
                success: function (json) {
                    cacheLastJson = $.extend(true, {}, json);

                    if (cacheLower != drawStart) {
                        json.data.splice(0, drawStart - cacheLower);
                    }
                    if (requestLength >= -1) {
                        json.data.splice(requestLength, json.data.length);
                    }

                    drawCallback(json);
                },
            });
        } else {
            json = $.extend(true, {}, cacheLastJson);
            json.draw = request.draw; // Update the echo for each response
            json.data.splice(0, requestStart - cacheLower);
            json.data.splice(requestLength, json.data.length);

            drawCallback(json);
        }
    };
};

// Register an API method that will empty the pipelined data, forcing an Ajax
// fetch on the next draw (i.e. `table.clearPipeline().draw()`)
$.fn.dataTable.Api.register('clearPipeline()', function () {
    return this.iterator('table', function (settings) {
        settings.clearCache = true;
    });
});

// #endregion

// #region Helper functions

function showAlert(message, type, title, icon) {
    // create a new alert element
    var alert = document.createElement("div");
    alert.className = "alert alert-dismissible alert-" + type;
    alert.role = "alert";

    var flexContainer = document.createElement("div");
    flexContainer.className = "d-flex";

    // create an icon element for the alert
    var iconElement = document.createElement("div");
    iconElement.className = "alert-icon";
    iconElement.innerHTML = icon;

    flexContainer.appendChild(iconElement);

    var titleAndMessageElement = document.createElement("div");

    // create a title element for the alert
    var titleElement = document.createElement("h4");
    titleElement.className = "alert-title";
    titleElement.textContent = title;

    // create a message element for the alert
    var messageElement = document.createElement("div");
    messageElement.className = "alert-message text-muted";
    messageElement.textContent = message;

    titleAndMessageElement.appendChild(titleElement);
    titleAndMessageElement.appendChild(messageElement);

    flexContainer.appendChild(titleAndMessageElement);

    // create a close button for the alert
    var closeButton = document.createElement("a");
    closeButton.className = "btn-close";
    closeButton.type = "button";
    closeButton.setAttribute("data-bs-dismiss", "alert");
    closeButton.setAttribute("aria-label", "Close");

    alert.appendChild(closeButton);
    alert.appendChild(flexContainer);

    // append the alert to the container
    var container = document.getElementById("alert-container");
    container.innerHTML = '';
    container.appendChild(alert);
}

// #endregion


function showSnackBar(message) {
    var x = document.getElementById("snackbar");
    x.innerHTML = message;
    x.className = "show";
    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 3000);
}

function showSnackBar(message, type, title, icon) {
    // create a new alert element
    var alert = document.createElement("div");
    alert.className = "alert alert-dismissible alert-" + type;
    alert.role = "alert";

    var flexContainer = document.createElement("div");
    flexContainer.className = "d-flex";

    // create an icon element for the alert
    var iconElement = document.createElement("div");
    iconElement.className = "alert-icon";
    iconElement.innerHTML = icon;

    flexContainer.appendChild(iconElement);

    var titleAndMessageElement = document.createElement("div");

    // create a title element for the alert
    var titleElement = document.createElement("h4");
    titleElement.className = "alert-title";
    titleElement.textContent = title;

    // create a message element for the alert
    var messageElement = document.createElement("div");
    messageElement.className = "alert-message text-muted";
    messageElement.textContent = message;

    titleAndMessageElement.appendChild(titleElement);
    titleAndMessageElement.appendChild(messageElement);

    flexContainer.appendChild(titleAndMessageElement);

    // create a close button for the alert
    var closeButton = document.createElement("a");
    closeButton.className = "btn-close";
    closeButton.type = "button";
    closeButton.setAttribute("data-bs-dismiss", "alert");
    closeButton.setAttribute("aria-label", "Close");

    alert.appendChild(closeButton);
    alert.appendChild(flexContainer);

    // append the alert to the container
    var x = document.getElementById("snackbar-advanced");
    x.innerHTML = '';
    x.appendChild(alert);

    x.className = "show";

    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 3000);
}

//async function fetchAPI(url, settings) {
//    var apiResult = {
//        success: false,
//        error: {
//            title: null,
//            detail: null
//        },
//        httpStatusCode: 0,
//        body: null
//    };

//    try {
//        const fetchResponse = await fetch(url, settings);

//        apiResult.success = fetchResponse.ok;
//        apiResult.httpStatusCode = fetchResponse.status;

//        // check if the response is ok
//        if (fetchResponse.ok) {
//            apiResult.body = await fetchResponse.json();
//        } else {
//            apiResult.body = await fetchResponse.json();
//            // response.status / title / detail
//            //showSnackBar("Error: " + result.detail);
//            showSnackBar(result.detail, "warning", `Error`, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');
//        }

//        return apiResult;
//    } catch (e) {
//        return e;
//    }
//}


async function completeProjectAllItems(data) {
    if (!confirm(`Confirm to complete all items of project ${data.name}`)) {
        return false;
    }

    const settings = {
        method: 'PATCH',
    };
    try {
        const fetchResponse = await fetch(`/api/projects/completeall/${data.id}`, settings);

        // check if the response is ok
        if (fetchResponse.ok) {
            //showSnackBar(`Success: ${data.name} all items completed successfully.`);
            showSnackBar(`${data.name} all items completed successfully.`, "success", `Success`, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');
        } else {
            const result = await fetchResponse.json();
            // response.status / title / detail
            //showSnackBar("Error: " + result.detail);
            showSnackBar(result.detail, "warning", result.title, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');
        }

        return fetchResponse.ok;
    } catch (e) {
        return e;
    }
}

async function deleteProject(data, table) {
    if (!confirm(`Confirm to delete project ${data.name}`)) {
        return false;
    }

    const settings = {
        method: 'DELETE',
    };
    try {
        const fetchResponse = await fetch(`/api/projects/delete/${data.id}`, settings);

        // check if the response is ok
        if (fetchResponse.ok) {
            showSnackBar(`${data.name} deleted successfully.`, "success", `Success`, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');

            // Refresh the table data from the server
            table.ajax.reload();
        } else {
            const result = await fetchResponse.json();
            // response.status / title / detail
            showSnackBar(result.detail, "warning", result.title, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');
        }

        return fetchResponse.ok;
    } catch (e) {
        return e;
    }
}

async function detailProject(data, table) {
    window.location.href = `/project/${data.id}`;

    return;

    const settings = {
        method: 'GET',
    };
    try {
        const fetchResponse = await fetch(`/api/projects/${data.id}`, settings);
        const result = await fetchResponse.json();

        // check if the response is ok
        if (fetchResponse.ok) {

            //modal-workperiod
            let modal = document.getElementById("modal-team");

            //// listen for the shown.bs.modal event
            //modal.addEventListener("show.bs.modal", function () {
            //    document.getElementById("WorkPeriodBindingModel_Name").value = "";
            //});

            $(`#${modal.id}`).modal('show');

        } else {
            // response.status / title / detail
            showSnackBar(result.detail, "warning", result.title, '<svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z"/><circle cx="12" cy="12" r="9" /><line x1="12" y1="8" x2="12.01" y2="8" /><polyline points="11 12 12 12 12 16 13 16" /></svg>');
        }

        return fetchResponse.ok;
    } catch (e) {
        return e;
    }
}