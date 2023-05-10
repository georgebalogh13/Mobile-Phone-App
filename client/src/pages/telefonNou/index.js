const form = document.querySelector("#telefon-form");

form.addEventListener("submit", (event) => {
  event.preventDefault();

  const brand = document.querySelector("#brandInput").value;
  const model = document.querySelector("#modelInput").value;
  const price = document.querySelector("#priceInput").value;

  const newPhone = {
    brand: brand,
    model: model,
    price: parseFloat(price),
  };

  fetch("https://localhost:44314/api/Phones", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(newPhone),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      console.log(data);
      alert("Telefonul a fost adaugat cu succes!");
      form.reset();
      window.location.href = `../detaliiTelefon/?id=${data.id}`;
    })
    .catch((error) => {
      console.error("There was a problem with the fetch operation:", error);
    });
});
