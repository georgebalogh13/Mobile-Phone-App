const url = "https://localhost:44314/api/Phones";

fetch(url)
  .then((response) => {
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    return response.json();
  })
  .then((data) => {
    console.log(data);
    // Clear the static phone list
    document.querySelector(".phone-list").innerHTML = "";
    // Generate the dynamic phone list
    // Generate the dynamic phone list
    data.forEach((phone) => {
      const listItem = `
      <li class="list-group-item d-flex justify-content-between align-items-center">
        ${phone.brand} ${phone.model}
        <span class="badge badge-primary badge-pill">$${phone.price.toFixed(
          2
        )}</span>
        <div>
          <button type="button" class="btn btn-sm btn-outline-primary mr-1"
            onclick="window.location.href = './src/pages/detaliiTelefon/index.html?id=${
              phone.id
            }'">Detalii</button>
          <button type="button" class="btn btn-sm btn-outline-danger" onclick="confirmDelete('${
            phone.id
          }')">Șterge</button>
        </div>
      </li>
    `;
      // Append each phone to the phone list
      document
        .querySelector(".phone-list")
        .insertAdjacentHTML("beforeend", listItem);
    });
  })

  .catch((error) => {
    console.error("There was a problem with the fetch operation:", error);
  });

function confirmDelete(id) {
  const modalHtml = `
      <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="deleteModalLabel">Ștergere Telefon</h5>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              Esti sigur ca doresti sa stergi acest telefon?
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-dismiss="modal">Nu</button>
              <button type="button" class="btn btn-danger" onclick="deletePhone('${id}')">Da</button>
            </div>
          </div>
        </div>
      </div>
    `;
  // Append the modal to the body
  document.body.insertAdjacentHTML("beforeend", modalHtml);
  // Show the modal
  $("#deleteModal").modal("show");
}

function deletePhone(id) {
  const deleteUrl = `https://localhost:44314/api/Phones/${id}`;
  fetch(deleteUrl, {
    method: "DELETE",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      location.reload();
    })
    .catch((error) => {
      console.error("There was a problem with the fetch operation:", error);
    });
}
