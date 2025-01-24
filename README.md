# Hacker News Best Stories API

This project is an ASP.NET Core Web API that retrieves the best `n` stories from the Hacker News API. The stories are sorted by their scores in descending order. The API integrates with the Hacker News endpoints and utilizes `HttpClient` for efficient HTTP requests and `MemoryCache` for caching data to reduce external API calls.

---

## Features

- Fetches the top `n` stories from the Hacker News API, ordered by score.
- Efficient caching using in-memory storage to reduce redundant external API calls.
- URLs for Hacker News API endpoints are configurable via `appsettings.json`.
- Built-in Swagger documentation for testing and exploring the API.

---

## How to Run the Application

### Prerequisites
- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- Optional: [Postman](https://www.postman.com/) or any API client to test the endpoints.

### Steps to Run
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. Update the configuration in `appsettings.json`:
   ```json
   {
     "HackerNewsApi": {
       "StoriesUrl": "https://hacker-news.firebaseio.com/v0/beststories.json",
       "ItemUrl": "https://hacker-news.firebaseio.com/v0/item/{0}.json"
     }
   }
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Navigate to Swagger at:
   ```
   http://localhost:5000/swagger
   ```

---

## API Endpoints

### 1. **GET `/api/BestStories`**
Fetch the top `n` stories based on their scores.

**Query Parameters:**
- `n` (optional): Number of stories to fetch. Default is `10`.

**Example Request:**
```http
GET http://localhost:5000/api/BestStories?n=5
```

**Example Response:**
```json
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
  },
  ...
]
```

---

## Assumptions

1. **Hacker News API Endpoints:**
   - The URLs for fetching story IDs and story details are defined in `appsettings.json` under the `HackerNewsApi` section.
   - These URLs are:
     - `StoriesUrl`: Endpoint to retrieve the list of best story IDs.
     - `ItemUrl`: Endpoint to fetch details of a specific story (formatted with `{0}` for the story ID).

2. **Caching:**
   - In-memory caching is used to reduce API calls to Hacker News.
   - Stories are cached for **5 minutes** to minimize load on the external API.

3. **Error Handling:**
   - The API handles potential failures when interacting with Hacker News, returning a 500 status code if something goes wrong.
   - Assumes that Hacker News API is generally available and responsive.

4. **Performance:**
   - The API is designed to handle a moderate load of requests. For high scalability, a distributed cache (e.g., Redis) could replace the in-memory cache.

5. **Story Count Limitation:**
   - The maximum number of stories that can be fetched (`n`) is only limited by the length of the story IDs list returned by Hacker News.

---

## Future Enhancements

1. **Distributed Cache:**
   - Replace in-memory caching with a distributed caching system (e.g., Redis) to support scaling across multiple instances.

2. **Pagination:**
   - Introduce pagination to allow fetching more stories in manageable chunks.

3. **Rate Limiting:**
   - Add rate limiting to protect the Hacker News API from being overloaded.

4. **Configuration Enhancements:**
   - Add support for overriding configurations via environment variables for flexibility in deployment.

5. **Retry Policies:**
   - Integrate retry logic for external API calls using libraries like Polly.

---

## Built With

- **ASP.NET Core 8**
- **Swagger**
- **HttpClient**
- **MemoryCache**

---

## Contact

For any issues or suggestions, feel free to reach out!

