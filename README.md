# .NET Core Invoice API

This API is using the Onion/Clean layered architecture with 6 layers

- CORE : Core Layer
- API : Presentation Layer
- DAL : Data Access Layer
- DAL.Tests : DAL Test Layer
- BL : Bussiness Logic Layer
- BL.Tests : BL Test Layer

Here all the layers mainly depends on Core layer. Core layer consists of interfaces which defines rules for all ther layers. (IoD)

The main point is, higher lavel classes depends only on abstractions not on implementations. In this way, for future development we can easily swap out the implementations because non of the classes depends on implementations. Also this makes classes more reusable and easily extended.

Fallowing design patterns are used

- Dependency Indjection
- Repositories
- Unit Of Work

Even though Entityframework itself if composed of Repositories and Unit Of Work, it is not designed test. It is not for direct use. We have to create repositories representing the logic of each repository independantly then put them together using unit of work.
