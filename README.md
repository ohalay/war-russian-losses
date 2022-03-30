# War russian losses

<!--- These are examples. See https://shields.io for others or to customize this set of shields. You might want to include dependencies, project status and licence info here --->
![GitHub repo size](https://img.shields.io/github/repo-size/scottydocs/README-template.md)
![GitHub contributors](https://img.shields.io/github/contributors/scottydocs/README-template.md)
![GitHub stars](https://img.shields.io/github/stars/scottydocs/README-template.md?style=social)
![GitHub forks](https://img.shields.io/github/forks/scottydocs/README-template.md?style=social)

Project is an `API` that allows `users` to get information about russian losses in Ukrainian-Russian war from day to day. API is base on GraphQL.

## Prerequisites

Before you begin, ensure you have met the following requirements:

* You have installed the latest version of `.NET`
* You have installed the latest version of `docker`
* You have installed or remote `postgres` server

## Installing War.RussianLosses.Api

To install `War.RussianLosses.Api`, follow these steps:

- Add ef core *postgre connection* string to user secrets
- Run `Update-Database` from ms vs or ef core .net tool
- Run insert script `Infrastructure\Migrations\Insert.sql`

## Using War.RussianLosses.Api

To use War.RussianLosses.Api, juts run from ms vs or vs code.

## Contributing to War.RussianLosses.Api
To contribute to `War.RussianLosses.Api`, follow these steps:

1. Fork this repository.
2. Create a branch: `git checkout -b main`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`
4. Push to the original branch: `git push origin <project_name>/<location>`
5. Create the pull request.

Alternatively see the GitHub documentation on [creating a pull request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## Contributors

Thanks to the following people who have contributed to this project:

You might want to consider using something like the [All Contributors](https://github.com/all-contributors/all-contributors) specification and its [emoji key](https://allcontributors.org/docs/en/emoji-key).

## Contact

If you want to contact me you can reach me at <olehhalay@ukr.net>.

## License
<!--- If you're not sure which open license to use see https://choosealicense.com/--->

This project uses the following license: [MIT](LICENSE.TXT).