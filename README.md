# WebHook Test
This project is used to demonstrate the concept of webhooks.

## The solution consists a 
- WebHookServer
  - Registers topics that a consumer is interested in subscribing to
  - Accepts events from Producers

- PlayerManagementService 
  - Handles the Registration and Deactivation of users

## Process Flow
1. WebHookService is started
2. PlayerManagementService is started
   - within the load routine, sends a request to the WebHookService to register the player registration and deactivate topics along with a callback url to the relevant sub service endpoint.
3. Producer publishes an event to the WebHookService e.g. Register User X
4. WebHookService notifies all subscribers to the topic by making a request to the callback url with the event payload
5. PlayerManagementService handles the request with the attached payload