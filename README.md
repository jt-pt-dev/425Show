# 425Show
#### Troubleshooting AAD B2C

This solution hopefully shows were we are currently stuck in our larger project.

We have a suite of related applications that are currently separate Razor Pages clients (all Core net5.0). In the future we will be building mobile clients.

All of these clients will talk to API backends (all Core net5.0). Each client has its own API and there is a shared API all clients use. Therefore, each client will need to access at a minimum two APIs.

The three app registrations are set up in B2C and the web client has been granted the scopes for accessing the two APIs.

B2C will only do authentication and control access to the APIs. Authorization inside the APIs will be a concern of the application and not B2C.

The web client index page is publicly accessible but the privacy page requires authentication. **This currently works**.

The privacy page has some links for testing the APIs and in this demo each API has a public and protected endpoint.

The public API calls work. The protected API calls fail with a 401 Unauthorized.

**This makes sense as currently we cannot seem to retrieve an access token**.

Challenges are:
1. Where is our access token?
2. Confirm we can call both APIs when we have the access token

(The appsettings.json files have placeholders for the various domains/ids and are stored in user secrets locally)
