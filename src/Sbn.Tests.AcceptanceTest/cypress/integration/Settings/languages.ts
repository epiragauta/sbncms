/// <reference types="Cypress" />
context('Languages', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Add language', () => {
      // For some reason the languages to chose fom seems to be translated differently than normal, as an example:
      // My system is set to EN (US), but most languages are translated into Danish for some reason
      // Aghem seems untranslated though?
      const name = "Aghem"; // Must be an option in the select box

     cy.sbnEnsureLanguageNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Languages"]).click();

    cy.sbnButtonByLabelKey("languages_addLanguage").click();

    cy.get('select[name="newLang"]').select(name);

    // //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');

    //Clean up
    cy.sbnEnsureLanguageNameNotExists(name);
   });

});
