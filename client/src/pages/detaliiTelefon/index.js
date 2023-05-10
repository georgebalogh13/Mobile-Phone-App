const urlParams = new URLSearchParams(window.location.search);
const id = urlParams.get("id");

const phoneName = document.getElementById("phone-name");
const phoneImage = document.getElementById("phone-image");
const phoneDescription = document.getElementById("phone-description");
const phonePrice = document.getElementById("phone-price");
const editButton = document.getElementById("edit-phone-btn");
const form = document.getElementById("editForm");
const brandInput = document.getElementById("brandInput");
const modelInput = document.getElementById("modelInput");
const priceInput = document.getElementById("priceInput");

fetch(`https://localhost:44314/api/Phones/${id}`)
  .then((response) => {
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    return response.json();
  })
  .then((data) => {
    phoneName.innerText = `${data.brand} ${data.model}`;
    phonePrice.innerText = data.price.toFixed(2);
    brandInput.value = data.brand;
    modelInput.value = data.model;
    priceInput.value = data.price.toFixed(2);
  })
  .catch((error) => {
    console.error("There was a problem with the fetch operation:", error);
  });

editButton.addEventListener("click", () => {
  $("#editModal").modal("show");
});

form.addEventListener("submit", (event) => {
  event.preventDefault();

  const brand = brandInput.value;
  const model = modelInput.value;
  const price = parseFloat(priceInput.value);

  const updatedPhone = {
    id: id,
    brand: brand,
    model: model,
    price: price,
  };

  fetch(`https://localhost:44314/api/Phones/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(updatedPhone),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.text(); // return plain text instead of json
    })
    .then((data) => {
      console.log(data);
      alert(data); // show response text in alert
      $("#editModal").modal("hide");
      location.reload();
    })
    .catch((error) => {
      console.error("There was a problem with the fetch operation:", error);
    });
});
