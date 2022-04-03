# War russian losses

[![Deploy to Heroku](https://github.com/ohalay/war-russian-losses/actions/workflows/deploy.yml/badge.svg)](https://github.com/ohalay/war-russian-losses/actions/workflows/deploy.yml)
![GitHub repo size](https://img.shields.io/github/repo-size/ohalay/war-russian-losses)
![GitHub contributors](https://img.shields.io/github/contributors/ohalay/war-russian-losses)
![GitHub stars](https://img.shields.io/github/stars/ohalay/war-russian-losses?style=social)
![GitHub forks](https://img.shields.io/github/forks/ohalay/war-russian-losses?style=social)

Project is an `API` that allows `users` to get information about russian losses in Ukrainian-Russian war from day to day. API is base on GraphQL. All date from [Ministry of Defence Ukraine](https://www.mil.gov.ua/)

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
- Run `InsertLossesTests` tests to insert losses (initial and delta if need)

## Using War.RussianLosses.Api
There are two option possible for using API with host - http://ukrainian-war.herokuapp.com

* `{host}/graphql` - graphql API with documented schema
* `{host}/img?from=<date>&to=<date>` - generate image with date range (date rage is optional)

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