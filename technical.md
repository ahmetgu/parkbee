#### 1. How would you improve the API to make it production-ready (bug fixes, security, performance, etc.)?
- implement authentication and authorization properly
- implement healtcheck endpoints for api
- define performance criteria and implement performance tests
- introduce static and dynamic code analysis and fix bugs related to security
- try to use less dependency in terms of public nuget packages as they will create potential security issues

#### 2. How would you make the API idempotent?
I'd say currently it is idempotent, obviously get methods are idempotent by nature. 
Trying to create a session with same inputs will not create a new session and deleting an already ended session will have no effect meaning system will be in the same state as before in both cases. 
Ignoring the fact that parking spot availability and hardware communication result might change for the negative scenarios. 
Another minor detail is that you may opt for returning the running session id for a successive post request but it's just cosmetics current behaviour is still idempotent.

#### 3. How would you approach the API authentication?
Let users authenticate against partner's choice of identity provider then validate authentication token agains the identity provider.
 
#### 4. What type of storage would you use for this service in production?
Judging by the requirements available to me I'd say both a relational database and no-sql database can be used.
I've separated active sessions and past sessions in database as ended session information is not required in session operations. 
Even with concurrent active session numbers around millions I would expect database performance to be acceptable and I would come up with a performance test scenario before committing.
Other option is to use a no-sql database and build a document around Garage object as root, which would also provide a natural order and structure as the API grows in number of functions.

#### 5. How would you deploy this API to production? Which infrastructure would you need for that (databases, messaging, etc)?
- A relational or document based db, would go for saas offerings if price is not an issue :) 
- A message queue/service bus again prefer a saas option. 
- Might need an in memory database solution depending on the performance.
- Would consider cloud native solution for the API and/or hardware communication if possible. Otherwise I'd use a kubernetes cluster. 
- If there are managed infrastructure either they need to be integrated with main observability tool that the cloud provider offers or alternative solution like prometheus stack etc. needed

#### 6. How would you optimize the API endpoints to guarantee low latency under the high load?
I would offload hardware communication from the session endpoints, use healtchecks for door status and keep them in memory
(this is for high load performance optimization, since I don't have any numbers available to me I will ignore cost efficiency)
And introduce messaging mechanism for door openings, session endpoints would simply be responsible from publishing a message to a service/event bus.

Both options explained in Q4 should provide enough performance on database level so the high load could be handled by load balancing and scaling API instances without a performance issue.
